/// <reference path="device-uuid.js" />
/**
	Programado por Edgar Rivera - Julio 2022 
*/

var intervalId = window.setInterval(function ()
{
	funcionInit();
}, 5000);

function removeClass()
{
	let element = document.getElementById('setflag');
	element.className = element.className.replace('btn btn-success btn-sm', 'btn btn-danger btn-sm');
}

function mySetter(lt, lg)
{
	var uuid = new DeviceUUID().get();
	document.getElementById("uuid").value = uuid;
	document.getElementById("lt").value = lt;
	document.getElementById("lg").value = lg;

	// CAMPOS COMERCIALES
	document.getElementById("uuid_c").value = uuid;
	document.getElementById("lt_c").value = lt;
	document.getElementById("lg_c").value = lg;

}



const funcionInit = () => {
	if (!"geolocation" in navigator) {
		return alert("Tu navegador no soporta el acceso a la ubicación. Intenta con otro!");
	}

	const
		$latitud = document.querySelector("#latitud"),
		$longitud = document.querySelector("#longitud"),
		$enlace = document.querySelector("#enlace"),
		$setflag = document.querySelector("#setflag"),
		$latitud_c = document.querySelector("#latitud_c"),
		$longitud_c = document.querySelector("#longitud_c"),
		$enlace_c = document.querySelector("#enlace_c"),
		$setflag_c = document.querySelector("#setflag_c")



	const onUbicacionConcedida = ubicacion => {
		const coordenadas = ubicacion.coords;
		$latitud.innerText = coordenadas.latitude;
		$longitud.innerText = coordenadas.longitude;
		// COMERCIAL
		$latitud_c.innerText = coordenadas.latitude;
		$longitud_c.innerText = coordenadas.longitude;
		mySetter(coordenadas.latitude, coordenadas.longitude);
		$setflag.innerText = "Ubicación Obtenida Exitosamente";
		$setflag_c.innerText = "Ubicación Obtenida Exitosamente";
		$enlace.href = `https://www.google.com/maps/@${coordenadas.latitude},${coordenadas.longitude},20z`;
		$enlace_c.href = `https://www.google.com/maps/@${coordenadas.latitude},${coordenadas.longitude},20z`;
		
	}
	const onErrorDeUbicacion = err => {

		$latitud.innerText = "Error obteniendo ubicación: " + err.message;
		$longitud.innerText = "Error obteniendo ubicación: " + err.message;
		$setflag.innerText = "Error obteniendo ubicación: " + err.message;
		removeClass();
	}

	const opcionesDeSolicitud = {
		enableHighAccuracy: true, // Alta precisión
		maximumAge: 0, // No queremos caché
		timeout: 15000 // Esperar solo 5 segundos
	};

	$latitud.innerText = "Cargando...";
	$longitud.innerText = "Cargando...";
	navigator.geolocation.getCurrentPosition(onUbicacionConcedida, onErrorDeUbicacion, opcionesDeSolicitud);

};
document.addEventListener("DOMContentLoaded", funcionInit);