<%@ Page Language="VB" CodeFile="default.aspx.vb" Inherits="pc"%>
<!DOCTYPE HTML>
<html lang="en-US">
<head>
	<meta charset="UTF-8">
	<title><%=pgTitulo%></title>
	
	<script src="/src/js/jquery/jquery-2.2.4.min.js"></script>
	<script src="/src/js/three.js-master/build/three.js"></script>
	
	<style type="text/css">
	
	</style>
</head>
<body>
	<script type="text/javascript">
	
	var scene, camera, renderer;
	var geometry, geometryText, material, materialText, mesh, meshText;

	init();
	animate();

	function init() {

		scene = new THREE.Scene();

		camera = new THREE.PerspectiveCamera( 75, window.innerWidth / window.innerHeight, 1, 10000 );
		camera.position.z = 1000;

		geometry = new THREE.BoxGeometry( 200, 200, 200 );
		geometryText = new THREE.TextGeometry( "WEBSAITO", {
			//font: '',//— THREE.Font.
			size: 100,//— Float. Size of the text.
			height: 55,//— Float. Thickness to extrude text. Default is 50.
			curveSegments: 30,//— Integer. Number of points on the curves. Default is 12.
			bevelEnabled: true, //— Boolean. Turn on bevel. Default is False.
			bevelThickness: 25, //— Float. How deep into text bevel goes. Default is 10.
			bevelSize: 10 //— Float. How far from text outline is bevel. Default is 8.
		});
		
		material = new THREE.MeshBasicMaterial( { color: 0xff0000, wireframe: true } );
		materialText = new THREE.MeshBasicMaterial( { color: 0x33FFcc, wireframe: false } );
		
		
		
		mesh = new THREE.Mesh( geometry, material );
		scene.add( mesh );

		renderer = new THREE.WebGLRenderer();
		renderer.setSize( window.innerWidth, window.innerHeight );

		document.body.appendChild( renderer.domElement );

	}

	function animate() {

		requestAnimationFrame( animate );

		mesh.rotation.x += 0.01;
		mesh.rotation.y += 0.02;

		renderer.render( scene, camera );

	}
	
	</script>
	
	<h1>TESTE</h1>
	
</body>
</html>