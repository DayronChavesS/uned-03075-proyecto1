
//al cargar la pagina
window.onload = function () {
    //se crearan todos los listener que detectan eventos especificos en la pagina
    CrearListenerCedula();
    CrearListenerTelefono();
    CrearListenerDescuento();
    CrearListenerSubmit();
}

function CrearListenerCedula() {
    var input;
    input = document.getElementById("input_cedula"); //se obtiene el elemento "input_cedula"
    input.addEventListener("keypress", function () { FormatearCedula(input); }); //se establece que se ejecute una funcion al tocar una tecla
}

function CrearListenerTelefono() {
    var input;
    input = document.getElementById("input_telefono"); //se obtiene el elemento "input_telefono"
    input.addEventListener("keypress", function () { FormatearTelefono(input); }); //se establece que se ejecute una funcion al tocar una tecla
}

function CrearListenerDescuento() {
    var inputSi;
    var inputNo;
    //se obtienen los elementos "input radioSi y radioNo"
    inputSi = document.getElementById("input_radioSi");
    inputNo = document.getElementById("input_radioNo");
    //se establece que se ejecuten funciones al hacer click sobre ellos
    inputSi.addEventListener("click", function () { DeterminarDescuento(inputSi); });
    inputNo.addEventListener("click", function () { DeterminarDescuento(inputNo); });
}

function CrearListenerSubmit() {
    var formulario;
    formulario = document.getElementById("formulario"); //se obtiene el elemento formulario
    formulario.addEventListener("submit", VerificarDatos); //se establece que se ejecute una funcion al hacer submit
}

//funcion que elige la forma en que se formatea la cedula
function FormatearCedula(input) {
    var datos_input = input.value; //se obtiene el valor del inupt
    var rango = /^[1-9]+$/; //expresion regular que contiene los numeros del 1 al 9

    //si el numero al principio es 0
    if (datos_input.charAt(0).match("0")) {
        FormatearFisicamente(input); //se formatea fisicamente
    }
    //si el numero al principio esta en el rango de 1 al 9
    else if (datos_input.charAt(0).match(rango)) {
        //se formatea juridicamente
        FormatearJuridicamente(input);
    }
}

//funcion que formatea la cedula de forma fisica
function FormatearFisicamente(input) {
    var datos_input = input.value; //se obtiene los datos del input

    //si el tamaño del input es 2
    if (datos_input.length === 2) {
        //se añade un guion
        input.value = input.value + "-";
    }
    //si el tamaño del input es 7
    if (datos_input.length === 7) {
        //se añade un guion
        input.value = input.value + "-";
    }

    //se modifica el html de la pagina para señalar al usuario que esta escribiendo una cedula fisica
    var html_TipoCedula = "<a>" + "Cedula Fisica" + "</a> </br>";
    document.getElementById("label_cedula").innerHTML = html_TipoCedula;
}

//funcion que formatea la cedula de forma juridica
function FormatearJuridicamente(input) {
    var datos_input = input.value; //se obtiene los datos del input
    //si el tamaño del input es 1
    if (datos_input.length === 1) {
        //se añade un guion
        input.value = input.value + "-";
    }
    //si el tamaño del input es 5
    if (datos_input.length === 5) {
        //se añade un guion
        input.value = input.value + "-";
    }

     //se modifica el html de la pagina para señalar al usuario que esta escribiendo una cedula juridica
    var html_TipoCedula = "<a>" + "Cedula Juridica" + "</a> </br>";
    document.getElementById("label_cedula").innerHTML = html_TipoCedula;
}

//funcion que formatea el numero de telefono
function FormatearTelefono(input) {
    var datos_input = input.value; //se obtiene los datos del input

    //si el tamaño del input es 4
    if (datos_input.length === 4) {
        //se añade un guion
        input.value = input.value + "-";
    }
}

//funcion que determina el descuento con base en los controles falsos.
function DeterminarDescuento(radiobutton) {
    var descuento;
    descuento = document.getElementById("input_descuento"); //se obtiene el elemento "input_descuento"

    var radioseleccionado = radiobutton.getAttribute("id"); //se obtene el ID del radiobutton recibido por parametro

    //si el ID era del RadioButton Si
    if (radioseleccionado === "input_radioSi") {
        //se modifica el valor del verdadero descuento a verdadero
        descuento.checked = true;
        descuento.value = true;
    }
    //si el ID era del RadioButton No
    else if (radioseleccionado === "input_radioNo") {
        //se modifica el valor del verdadero descuento a falso
        descuento.checked = false;
        descuento.value = false;
    }
}

//funcion que verifica algunos datos del formulario, es capaz de cancelar el submit.
function VerificarDatos(e) {
    var cedula;
    cedula = document.getElementById("input_cedula"); //se obtiene el elemento "input_cedula"

    var telefono;
    telefono = document.getElementById("input_telefono"); //se obtiene el elemento "input_telefono"

    EliminarGuiones(cedula, telefono); //se envia a eliminar los guiones de los inputs

    //se obtiene los valores (ahora limpios) de los inputs
    var valor_cedula = cedula.value;
    var valor_telefono = telefono.value;
    var regexnumeros = /^[0-9]+$/; //expresion regular que contiene los numeros del 0 al 9

    if (!valor_cedula.match(regexnumeros)) //si el valor de la cedula no coincide con la expresion regular
    {
        //se alerta al usuario y se cancela el submit
        alert("El formato de la cedula no es correcto, ingrese solo numeros.")
        e.preventDefault();
    }

    if (!valor_telefono.match(regexnumeros)) //si el valor del telefono no coincide con la expresion regular
    {
         //se alerta al usuario y se cancela el submit
        alert("El formato del telefono no es correcto, ingrese solo numeros.");
        e.preventDefault();
    }
}

//funcion que elimina los guiones de la cedula y telefono
function EliminarGuiones(cedula, telefono) {

    var cedula_limpia = "";
    var telefono_limpio = "";

    var regexnumeros = /^[0-9]+$/; //expresion regular que contiene los numeros del 0 al 9

    //ciclo determinado por el largo de la cedula
    for (var i = 0; i < cedula.value.length; i++) {
        //se revisa si el caracter por el cual iteramos es un numero
        if (cedula.value.charAt(i).match(regexnumeros)) {
            //si lo es, se concatena al nuevo valor limpio.
            cedula_limpia = cedula_limpia.concat(cedula.value.charAt(i));
        }
    }

    //ciclo determinado por el largo del telefono
    for (var i = 0; i < telefono.value.length; i++) {
         //se revisa si el caracter por el cual iteramos es un numero
        if (telefono.value.charAt(i).match(regexnumeros)) {
            //si lo es, se concatena al nuevo valor limpio.
            telefono_limpio = telefono_limpio.concat(telefono.value.charAt(i));
        }
    }

    //se remplazan los valores originales por los valores limpios.
    cedula.value = cedula_limpia;
    telefono.value = telefono_limpio;
}
