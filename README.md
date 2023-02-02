# 2D-Animation-Using-3D-Tools
An independent study course focused on researching non-photorealistic rendering techniques to mimic 2D hand drawn animations using 3D models 

Unity Version: 5.6.1f1  
Platform: Windows Desktop  
Playthrough:  

To create the outline effect, a two pass shader was created. The first pass is responsible for the outline of the object. In the first pass, the front face culling is turned on, causing any surface facing the camera to be removed. A 3D object consists of points, edges that connect points, and surfaces that connect edges. Each surface has a vector called a normal vector. The normal is a vector perpendicular to the surface, and signifies the front of the surface. To create the outline the first pass expands the surfaces along their normal vector increasing the size of the 3D object. The object is then rendered to the screen given a color for the outline. In the second pass, the back face culling is turned on, and the object is drawn at its normal size. The overlap from the first pass becomes the shadow around the object. To apply this shader in unity the user must create a material, select the outline shader, and then attach it to the object that needs an outline.

For an object shadow, a Unity Projector is used. It is attached to the parent of the object and set directly above the object facing downwards. Once the projector is created it is set to ignore the layer that the object is on so the shadow is only cast on the ground. The cel-shading effect is accomplished by projecting a ramp texture with only values, shaded and unshaded, directly below the object. 

