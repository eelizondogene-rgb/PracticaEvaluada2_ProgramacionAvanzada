(() => {
    const Producto = {
        tabla: null,

        init() {
            this.tabla = new DataTable('#tblProducto', {
                ajax: { url: '/Producto/ObtenerProductos', dataSrc: '' },
                columns: [
                    { data: 'id' },
                    { data: 'nombre', render: DataTable.render.text() },
                    { data: 'categoriaNombre', render: DataTable.render.text() },
                    {
                        data: null,
                        orderable: false,
                        searchable: false,
                        render: (_data, _type, row) => `<div class="table-actions">
                            <button type="button" class="btn-action btn-action-edit btn-editar" data-id="${row.id}" title="Editar"><i class="bi bi-pencil-fill"></i></button>
                            <button type="button" class="btn-action btn-action-delete btn-eliminar" data-id="${row.id}" title="Eliminar"><i class="bi bi-trash3-fill"></i></button>
                        </div>`
                    }
                ],
                language: { url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json' }
            });

            $('#formCrearProducto, #formEditarProducto').on('submit', e => {
                e.preventDefault();
                this.guardar($(e.currentTarget));
            });

            $('#tblProducto').on('click', '.btn-editar', e => this.cargar($(e.currentTarget).data('id')));
            $('#tblProducto').on('click', '.btn-eliminar', e => this.eliminar($(e.currentTarget).data('id')));

            $('#modalCrearProducto, #modalEditarProducto').on('hidden.bs.modal', function () {
                $(this).find('form')[0].reset();
            });
        },

        guardar(form) {
            if (!form[0].reportValidity()) return;
            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: respuesta => {
                    if (!respuesta.esCorrecto) return this.aviso(respuesta.mensaje, 'error');
                    this.aviso(respuesta.mensaje, 'success');
                    bootstrap.Modal.getInstance(form.closest('.modal')[0])?.hide();
                    this.tabla.ajax.reload();
                },
                error: xhr => this.error(xhr)
            });
        },

        cargar(id) {
            $.get(`/Producto/ObtenerProductoPorId?id=${encodeURIComponent(id)}`)
                .done(producto => {
                    const form = $('#formEditarProducto');
                    form.find('[name="Id"]').val(producto.id);
                    form.find('[name="Nombre"]').val(producto.nombre);
                    form.find('[name="CategoriaId"]').val(producto.categoriaId);
                    bootstrap.Modal.getOrCreateInstance(document.getElementById('modalEditarProducto')).show();
                })
                .fail(xhr => this.error(xhr));
        },

        eliminar(id) {
            Swal.fire({ title: '¿Eliminar producto?', icon: 'warning', showCancelButton: true, confirmButtonText: 'Sí, eliminar', cancelButtonText: 'Cancelar' })
                .then(resultado => {
                    if (!resultado.isConfirmed) return;
                    const token = $('#formCrearProducto input[name="__RequestVerificationToken"]').val();
                    $.ajax({
                        url: '/Producto/EliminarProducto',
                        type: 'POST',
                        data: { id, __RequestVerificationToken: token },
                        success: respuesta => {
                            this.aviso(respuesta.mensaje, respuesta.esCorrecto ? 'success' : 'error');
                            if (respuesta.esCorrecto) this.tabla.ajax.reload();
                        },
                        error: xhr => this.error(xhr)
                    });
                });
        },

        aviso(mensaje, icono) {
            Swal.fire({ title: icono === 'success' ? '¡Listo!' : 'Oops...', text: mensaje, icon: icono });
        },

        error(xhr) {
            const errores = xhr.responseJSON?.errors;
            const mensaje = errores ? Object.values(errores).flat().join('\n') : 'Ocurrió un error al procesar la solicitud.';
            this.aviso(mensaje, 'error');
        }
    };

    $(document).ready(() => Producto.init());
})();
