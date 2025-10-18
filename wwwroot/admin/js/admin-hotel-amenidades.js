let currentHotelId = null;

function closeAmenidadesModal() {
  document.getElementById("modalAmenidades").style.display = "none";
  currentHotelId = null;
}

async function verAmenidades(hotelId) {
  currentHotelId = hotelId;
  const modal = document.getElementById("modalAmenidades");
  const modalBody = document.getElementById("modalAmenidadesBody");
  modalBody.innerHTML = "Cargando...";
  modal.style.display = "block";

  try {
    const response = await fetch(`/api/hotelamenidadesapi/hotel/${hotelId}`);
    let amenidades = [];
    if (response.ok) {
      const result = await response.json();
      amenidades = result.data || [];
    } else if (response.status === 404) {
      amenidades = [];
    } else {
      throw new Error("Error al obtener amenidades");
    }

    modalBody.innerHTML = `
      <div>
        <ul id="listaAmenidades">
          ${amenidades.map(a => `
            <li>
              ${a.Amenidad}
              <button onclick="eliminarAmenidad(${a.AmenidadHotelId})"
                      class="btn btn-sm btn-danger" style="margin-left:10px;">🗑️</button>
            </li>
          `).join("")}
        </ul>
        <hr>
        <div style="display:flex; gap:10px; align-items:center;">
          <input type="text" id="nuevaAmenidad" class="form-control" placeholder="Nueva amenidad...">
          <button onclick="agregarAmenidad(${hotelId})" class="btn btn-success">Agregar</button>
        </div>
      </div>
    `;
  } catch (err) {
    modalBody.innerHTML = `<p class="text-danger">${err.message}</p>`;
  }
}

async function agregarAmenidad(hotelId) {
  const input = document.getElementById("nuevaAmenidad");
  const nombre = input.value.trim();
  if (!nombre) {
    alert("Por favor ingresa una amenidad.");
    return;
  }

  const nueva = { HotelId: hotelId, Amenidad: nombre };

  try {
   const res = await fetch("/api/hotelamenidadesapi", {
  method: "POST",
  headers: { "Content-Type": "application/json" },
  body: JSON.stringify(nueva)
});

const data = await res.json(); // leer respuesta del backend
if (!res.ok) {
    console.error(data); // muestra el error completo en la consola
    throw new Error(data?.message || "No se pudo agregar la amenidad");
}

    if (!res.ok) throw new Error("No se pudo agregar la amenidad");

    input.value = "";
    verAmenidades(hotelId);
  } catch (err) {
    alert(err.message);
  }
}

async function eliminarAmenidad(id) {
  if (!confirm("¿Seguro que deseas eliminar esta amenidad?")) return;

  try {
    const res = await fetch(`/api/hotelamenidadesapi/${id}`, { method: "DELETE" });
    if (!res.ok) throw new Error("Error al eliminar la amenidad");
    verAmenidades(currentHotelId);
  } catch (err) {
    alert(err.message);
  }
}