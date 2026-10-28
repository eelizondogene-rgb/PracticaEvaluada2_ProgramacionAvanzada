(() => {

    const Categoria = {
        tabla: null,

        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },

        /**
         * Inicializa la tabla de categorias. Los datos se cargan por AJAX desde el controlador.
         * Las columnas de texto usan render.text() para escapar HTML y evitar XSS.
         */
        inicializarTabla() {
            this.tabla = new DataTable('#tblCategoria', {
                ajax: {
                    url: '/Categoria/ObtenerCategorias',
                    dataSrc: '',
                    type: 'GET'
                },
                columns: [
                    { data: 'id' },
                    { data: 'nombre', render: DataTable.render.text() },
                    {
                        data: null,
                        title: 'Acciones',
                        className: 'dt-actions-cell',
                        orderable: false,
                        searchable: false,
                        render: (data, type, row) => {
                            return `<div class="table-actions">
                                        <button type="button" class="btn-action btn-action-edit btn-editar" data-id="${row.id}" aria-label="Editar categoría" title="Editar">
                                            <i class="bi bi-pencil-fill" aria-hidden="true"></i>
                                        </button>
                                        <button type="button" class="btn-action btn-action-delete btn-eliminar" data-id="${row.id}" aria-label="Eliminar categoría" title="Eliminar">
                                            <i class="bi bi-trash3-fill" aria-hidden="true"></i>
                                        </button>
                                    </div>`;
                        }
                    }
                ],
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
                }
            });
        },

        /**
         * Registra los eventos de los botones de los modales y de las acciones de la tabla.
         */
        registrarEventos() {
            $('#btnGuardarCategoria').on('click', () => Categoria.guardarCategoria());
            $('#btnEditarCategoria').on('click', () => Categoria.editarCategoria());

            // Delegacion de eventos: las filas se generan dinamicamente por DataTables
            $('#tblCategoria').on('click', '.btn-eliminar', function () {
                Categoria.eliminarCategoria($(this).data('id'));
            });
            $('#tblCategoria').on('click', '.btn-editar', function () {
                Categoria.cargarCategoria($(this).data('id'));
            });

            // Limpia los formularios al cerrar los modales para no arrastrar datos
            $('#modalCrearCategoria').on('hidden.bs.modal', () => $('#formCrearCategoria')[0].reset());
            $('#modalEditarCategoria').on('hidden.bs.modal', () => $('#formEditarCategoria')[0].reset());
        },

        /**
         * Envia el formulario de creacion al servidor.
         */
        guardarCategoria() {
            Categoria.enviarFormulario($('#formCrearCategoria'), $('#modalCrearCategoria'));
        },

        /**
         * Envia el formulario de edicion al servidor.
         */
        editarCategoria() {
            Categoria.enviarFormulario($('#formEditarCategoria'), $('#modalEditarCategoria'));
        },

        /**
         * Flujo comun de crear y editar: valida en el navegador, envia el formulario
         * y muestra el resultado de las reglas de negocio.
         * @param {jQuery} form Formulario a enviar.
         * @param {jQuery} modal Modal que contiene el formulario.
         */
        enviarFormulario(form, modal) {
            if (!form[0].reportValidity()) {
                return;
            }


            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (respuesta) { // Las reglas de negocio son procesos correctos, se informan con esCorrecto, no con excepciones
                    if (respuesta.esCorrecto) {
                        Swal.fire({
                            title: "¡Listo!",
                            text: respuesta.mensaje,
                            icon: "success"
                        });
                        modal.modal('hide');
                        Categoria.tabla.ajax.reload();
                    }
                    else {
                        Swal.fire({
                            icon: "error",
                            title: "Oops...",
                            text: respuesta.mensaje
                        });
                    }
                },
                error: function (xhr) { // Errores de validacion (400) o no controlados
                    Categoria.mostrarError(xhr);
                }
            });
        },

        /**
         * Pide confirmacion y elimina la categoria.
         * @param {number} id Id de la categoria.
         */
        eliminarCategoria(id) {
            Swal.fire({
                title: "¿Estás seguro?",
                text: "Se eliminará la categoría. No podrás revertir esta operación.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminar",
                cancelButtonText: "Cancelar"
            }).then((result) => {
                if (!result.isConfirmed) {
                    return;
                }

                $.ajax({
                    url: `/Categoria/EliminarCategoria?id=${encodeURIComponent(id)}`,
                    type: 'DELETE',
                    success: function (respuesta) {
                        if (respuesta.esCorrecto) {
                            Swal.fire({
                                title: "¡Listo!",
                                text: respuesta.mensaje,
                                icon: "success"
                            });
                            Categoria.tabla.ajax.reload();
                        }
                        else {
                            Swal.fire({
                                icon: "error",
                                title: "Oops...",
                                text: respuesta.mensaje
                            });
                        }
                    },
                    error: function (xhr) {
                        Categoria.mostrarError(xhr);
                    }
                });
            });
        },

        /**
         * Obtiene la categoria por id, llena el formulario de edicion y abre el modal.
         * @param {number} id Id de la categoria.
         */
        cargarCategoria(id) {
            $.get(`/Categoria/ObtenerCategoriaPorId?id=${encodeURIComponent(id)}`)
                .done(function (resultado) {
                    if (resultado == null) {
                        Swal.fire({
                            icon: "error",
                            title: "Oops...",
                            text: "No se encontró la categoría seleccionada."
                        });
                        return;
                    }

                    $('#formEditarCategoria #Id').val(resultado.id);
                    $('#formEditarCategoria #Nombre').val(resultado.nombre);

                    $('#modalEditarCategoria').modal('show');
                })
                .fail(function (xhr) {
                    Categoria.mostrarError(xhr);
                });
        },

        /**
         * Muestra un error de la peticion. Si es un 400 con ModelState, lista los mensajes de validacion.
         * @param {jqXHR} xhr Respuesta fallida de la peticion.
         */
        mostrarError(xhr) {
            let mensaje = "Ocurrió un error inesperado. Intenta de nuevo.";

            if (xhr.status === 400 && xhr.responseJSON) {
                const errores = Object.values(xhr.responseJSON.errors ?? xhr.responseJSON).flat();
                if (errores.length > 0) {
                    mensaje = errores.join('\n');
                }
            }

            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: mensaje
            });
        }
    };

    $(document).ready(() => Categoria.init());

})();