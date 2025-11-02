window.HotelCaracteristicasModule = {
    apiBase: '/api',
    currentHotelId: null,

    // Abrir modal y cargar características
    async verCaracteristicas(hotelId) {
        this.currentHotelId = hotelId;

        const modal = document.getElementById("modalCaracteristicasHotel");
        const modalBody = document.getElementById("modalCaracteristicasHotelBody");

        modalBody.innerHTML = "Cargando...";
        modal.style.display = "block";

        try {
            const response = await fetch(`${this.apiBase}/hotelcaracteristicasapi/hotel/${hotelId}`);
            let caracteristicas = [];

            if (response.ok) {
                const result = await response.json();
                caracteristicas = result.data || [];
            } else if (response.status === 404) {
                caracteristicas = [];
            } else {
                throw new Error("Error al obtener características");
            }

            if (caracteristicas.length === 0) {
                modalBody.innerHTML = `
                    <p>No hay características registradas para este hotel.</p>
                    <div style="display:flex; gap:10px; align-items:center; margin-top:10px;">
                        <input type="text" id="nuevaCaracteristicaHotel" class="form-control" placeholder="Nueva característica...">
                        <button onclick="window.HotelCaracteristicasModule.agregarCaracteristica()"
                                class="btn btn-success">Agregar</button>
                    </div>
                `;
            } else {
                modalBody.innerHTML = `
                    <div>
                        <ul id="listaCaracteristicasHotel">
                            ${caracteristicas.map(c => `
                                <li>
                                    ${c.caracteristica || c.Caracteristica}
                                    <button onclick="window.HotelCaracteristicasModule.eliminarCaracteristica(${c.id || c.Id})"
                                            class="btn btn-sm btn-danger" style="margin-left:10px;">🗑️</button>
                                </li>
                            `).join("")}
                        </ul>
                        <hr>
                        <div style="display:flex; gap:10px; align-items:center;">
                            <input type="text" id="nuevaCaracteristicaHotel" class="form-control" placeholder="Nueva característica...">
                            <button onclick="window.HotelCaracteristicasModule.agregarCaracteristica()"
                                    class="btn btn-success">Agregar</button>
                        </div>
                    </div>
                `;
            }

        } catch (err) {
            modalBody.innerHTML = `<p class="text-danger">${err.message}</p>`;
        }
    },

    // Agregar nueva característica
    async agregarCaracteristica() {
        const input = document.getElementById("nuevaCaracteristicaHotel");
        const nombre = input.value.trim();

        if (!nombre) {
            alert("Por favor ingresa una característica.");
            return;
        }

        const nueva = { HotelId: this.currentHotelId, Caracteristica: nombre };

        try {
            const res = await fetch(`${this.apiBase}/hotelcaracteristicasapi`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(nueva)
            });

            const data = await res.json();
            if (!res.ok) {
                console.error(data);
                throw new Error(data?.message || "No se pudo agregar la característica");
            }

            input.value = "";
            await this.verCaracteristicas(this.currentHotelId);

        } catch (err) {
            alert(err.message);
        }
    },

    // Eliminar característica
    async eliminarCaracteristica(id) {
        if (!confirm("¿Seguro que deseas eliminar esta característica?")) return;

        try {
            const res = await fetch(`${this.apiBase}/hotelcaracteristicasapi/${id}`, { method: "DELETE" });
            if (!res.ok) throw new Error("Error al eliminar la característica");
            await this.verCaracteristicas(this.currentHotelId);
        } catch (err) {
            alert(err.message);
        }
    },

    // Cerrar modal
    closeModal() {
        const modal = document.getElementById("modalCaracteristicasHotel");
        modal.style.display = "none";
        this.currentHotelId = null;
    }
};