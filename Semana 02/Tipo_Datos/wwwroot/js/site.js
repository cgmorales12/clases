
var unCliente = () => {
    var clienteid = document.getElementById("ClientesModelId").value;
    //fetch("/api/ProductosApi/"+ clienteid)

    /*fetch(`/api/ClientesApi/${clienteid}`)
        .then(
            uncliente => {
                if (!uncliente.ok) {
                    alert("Error al obtener el cliente")
                }
                return uncliente.json();
            })
        .then(datos => {
            console.log(datos)
            document.getElementById("Correo").value = datos.email;
            document.getElementById("Cedula_RUC").value = datos.cedula_RUC;
            document.getElementById("Telefono").value = datos.telefono;
            document.getElementById("Direccion").value = datos.direccion;
        }).catch(
            error => {
                alert("Ocucion un error:", error)
            }
        )*/

    $.get(`/api/ClientesApi/${clienteid}`, (uncliente) => {
        document.getElementById("Correo").value = uncliente.email;
        document.getElementById("Cedula_RUC").value = uncliente.cedula_RUC;
        document.getElementById("Telefono").value = uncliente.telefono;
        document.getElementById("Direccion").value = uncliente.direccion;
    })
}

var Lista_Productos = () => {
    $.get(`/api/ProductosApi`,async (listaproductos) => {
        html = "";
        $.each(listaproductos, (index, producto) => {
            html += `<tr >
                <td> ${producto.nombre} </td>
                <td> ${producto.precio} </td>
                <td> <input type='number' min="1" value="0" id="qty_${producto.id}"/> </td>
                <td> <button type="button"
                data-id="${producto.id}"
                data-nombre="${producto.nombre}"
                data-precio="${producto.precio}"
                onclick="cargarproducto(this)"
                class="btn-success">+</button> </td>
            `;
        })
        await $("#Lista_prodcutos").html(html)
    })
}

var cargarproducto = (producto) => {
    const id = producto.dataset.id
    const nombre = producto.dataset.nombre
    const precio = parseFloat(producto.dataset.precio)
    const cantidad_Modal = parseInt(document.getElementById(`qty_${id}`).value)
    if (cantidad_Modal <= 0 || isNaN(cantidad_Modal)) {
        alert("Inngrese una cantidad del producto valida")
        return;
    }
    const $tbody = $("#productosTable tbody")
    let $fila = $tbody.find(`tr[data-id="${id}"]`)
    if ($fila.length) {
        const $cantidadInput = $fila.find('input[name="Cantidad[]"]')
        const cantidadActual = parseInt($cantidadInput.val()) || 0
        const nuevaCantidad = cantidadActual + cantidad_Modal
        $cantidadInput.val(nuevaCantidad)
        const nuevoMonto = precio * nuevaCantidad 
        $fila.find('input[name="Monto[]]"').val(nuevoMonto.toFixed(2))
    } else {
        const monto = precio * cantidad_Modal
        const filaHtml = `
        <tr data-id="${id}">
            <td>${nombre}</td>
            <td><input type="number" name="Precio[]" step="0.01" min="0" value="${precio}"></td>
            <td><input type="number" name="Cantidad[]" min="1" value="${cantidad_Modal}"></td>
            <td><input type="number" name="Monto[]" step="0.01" min="0" readonly value="${monto}"></td>
            <td><button type="button" class="btn-remove">X</button></td>
        </tr>
        `;
        $tbody.append(filaHtml)
    }
}
$(document).on("click", ".btn-remove", function (){
    $(this).closest("tr").remove()
})

var crear_venta = async () => {

    const clienteId = parseInt(document.getElementById("ClientesModelId").value) || 0
    const metodo_pago = document.getElementById("metodoPago").value || ""

    if (clienteId == 0 || !clienteId) {
        alert("seleccion un cliente")
        return
    }

    const items = []
    let subTotal = 0

    $("#productosTable tbody tr[data-id]").each(function () {
        const $tr = $(this)
        const id = parseInt($tr.data("id"),10)
        if (!id) {
            alert("Ocurrio un error al guardar")
            return
        }
        const nombre = $tr.find("td").eq(0).text().trim()
        const precio = parseFloat($tr.find('input[name="Precio[]"]').val() || 0)
        const cantidad = parseFloat($tr.find('input[name="Cantidad[]"]').val() || 0)
        const monto = parseFloat($tr.find('input[name="Monto[]"]').val() || 0)

        if (cantidad > 0 && precio > 0) {
            items.push({
                ProductosModelId: id,
                Nombre: nombre,
                Precio: precio,
                Cantidad: cantidad,
                Monto: parseFloat((cantidad * precio).toFixed(2))
            })
            subTotal += cantidad * precio
        }
    })
    subTotal = parseFloat(subTotal.toFixed(2))
    const total = subTotal   //falta el descuento
    const venta = {
        Fecha_venta : new Date().toISOString(),
        codigo_venta : "",
        Notas: "----",
        SubTotal : subTotal,
        Estado_Venta : "COMPLETA",
        Descuento : 0,
        Total_Venta: total,
        Metodo_Pago : metodo_pago,
        ClientesModelId : clienteId,
        Productos_vendidos : items
    }

    try {
        const respusta = await fetch("/api/ventasAPI", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(venta)
        });
        /*if (!respusta.ok) {
            const error = await respusta.text()
            throw new Error(error || "No se pudo realizar la venta")
        }*/
        const creada = await respusta.json()
        console.log(creada)
        alert("Venta creada exitoddamente.")
        await imprimri_factura()


        $("#productosTable tbody").innerHTML("");
    } catch (e) {
        console.log(JSON.stringify(e))
        alert("Error al guardar la venta" + e.message)
    }
}

var imprimri_factura = () => {
    var contenido = document.getElementById("imprimir").innerHTML;
        var contenidoOriginal = document.body.innerHTML;
        document.body.innerHTML = contenido;
        window.print();
        document.body.innerHTML = contenidoOriginal;
    
}