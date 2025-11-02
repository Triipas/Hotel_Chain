window.HabitacionAmenidadesModule = {
    apiBase: '/api',
    currentHabitacionId: null,

    async verAmenidades(habitacionId) {
        this.currentHabitacionId = habitacionId;
        const modal = document.getElementById("modalAmenidadesHabitacion");
        const modalBody = document.getElementById("modalAmenidadesHabitacionBody");

        modalBody.innerHTML = "Cargando...";
        modal.style.display = "block";

        try {
            const response = await fetch(`${this.apiBase}/habitacionamenidadesapi/habitacion/${habitacionId}`);
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
                <ul id="listaAmenidadesHabitacion">
                    ${amenidades.length > 0
                        ? amenidades.map(a => `
                            <li>
                                ${a.Amenidad}
                                <button onclick="window.HabitacionAmenidadesModule.eliminarAmenidad(${a.Id})"
                                        class="btn btn-sm btn-danger" style="margin-left:10px;">🗑️</button>
                            </li>
                        `).join("")
                        : `<p>No hay amenidades registradas para esta habitación.</p>`}
                </ul>
                <hr>
                <div style="display:flex; gap:10px; align-items:center;">
                    <input type="text" id="nuevaAmenidadHabitacion" class="form-control" placeholder="Nueva amenidad...">
                    <button onclick="window.HabitacionAmenidadesModule.agregarAmenidad()"
                            class="btn btn-success">Agregar</button>
                </div>
            `;

        } catch (err) {
            modalBody.innerHTML = `<p class="text-danger">${err.message}</p>`;
        }
    },

    async agregarAmenidad() {
        const input = document.getElementById("nuevaAmenidadHabitacion");
        const nombre = input.value.trim();
        if (!nombre) {
            alert("Por favor ingresa una amenidad.");
            return;
        }

        const nuevaAmenidad = { HabitacionId: this.currentHabitacionId, Amenidad: nombre };

        try {
            const res = await fetch(`${this.apiBase}/habitacionamenidadesapi`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(nuevaAmenidad)
            });

            const data = await res.json();
            if (!res.ok) {
                console.error(data);
                throw new Error(data?.message || "No se pudo agregar la amenidad");
            }

            input.value = "";
            await this.verAmenidades(this.currentHabitacionId);
        } catch (err) {
            alert(err.message);
        }
    },

    async eliminarAmenidad(id) {
        if (!confirm("¿Seguro que deseas eliminar esta amenidad?")) return;

        try {
            const res = await fetch(`${this.apiBase}/habitacionamenidadesapi/${id}`, { method: "DELETE" });
            if (!res.ok) throw new Error("Error al eliminar la amenidad");
            await this.verAmenidades(this.currentHabitacionId);
        } catch (err) {
            alert(err.message);
        }
    },

    closeModal() {
        const modal = document.getElementById("modalAmenidadesHabitacion");
        modal.style.display = "none";
        this.currentHabitacionId = null;
    }
};