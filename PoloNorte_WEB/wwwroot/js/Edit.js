//Ver Create.js para comentarios (contiene la misma programacion, esta es una version reducida).
window.onload = function () {
    CrearListenerTelefono();
    CrearListenerDescuento();
    CrearListenerSubmit();
}
function CrearListenerTelefono() {
    var input;
    input = document.getElementById("input_telefono");
    input.addEventListener("keypress", function () { FormatearTelefono(input); });
}

function CrearListenerDescuento() {
    var inputSi;
    var inputNo;
    inputSi = document.getElementById("input_radioSi");
    inputNo = document.getElementById("input_radioNo");
    inputSi.addEventListener("click", function () { DeterminarDescuento(inputSi); });
    inputNo.addEventListener("click", function () { DeterminarDescuento(inputNo); });
}

function CrearListenerSubmit() {
    var formulario;
    formulario = document.getElementById("formulario");
    formulario.addEventListener("submit", VerificarDatos);
}

function FormatearTelefono(input) {
    var datos_input = input.value;

    if (datos_input.length === 4) {
        input.value = input.value + "-";
    }
}

function DeterminarDescuento(radiobutton) {
    var descuento;
    descuento = document.getElementById("input_descuento");

    var radioseleccionado = radiobutton.getAttribute("id");

    if (radioseleccionado === "input_radioSi") {
        descuento.checked = true;
        descuento.value = true;
    }
    else if (radioseleccionado === "input_radioNo") {
        descuento.checked = false;
        descuento.value = false;
    }
}

function VerificarDatos(e) {
    var telefono;
    telefono = document.getElementById("input_telefono");

    EliminarGuiones(telefono);

    var valor_telefono = telefono.value;
    var regexnumeros = /^[0-9]+$/;

    if (!valor_telefono.match(regexnumeros)) {
        alert("El formato del telefono no es correcto, ingrese solo numeros.");
        e.preventDefault();
    }
}

function EliminarGuiones(telefono) {

    var telefono_limpio = "";

    var regexnumeros = /^[0-9]+$/;

    for (var i = 0; i < telefono.value.length; i++) {
        if (telefono.value.charAt(i).match(regexnumeros)) {
            telefono_limpio = telefono_limpio.concat(telefono.value.charAt(i));
        }
    }

    telefono.value = telefono_limpio;
}