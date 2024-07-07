s*****************************************
*      Luma Based Ambient Occlusion     *
*       by Ramiro Oliva (Kronnect)      * 
*              README FILE              *
*****************************************


What is LBAO?
-------------

Luma Based Ambient Occlusion creates a simulated occlusion effect based solely on color information.
It's aimed at 2D scenes, where no depth information is available, providing some sort of depth shading or feeling to flat 2D images.
It can also be used on 3D scenes as well although the results will be mostly incorrect. For 3D scenes, use HBAO or other depth-based AO solutions.
 

How does it work?
-----------------

LBAO checks the color difference between each pixel and its neighbours. Like in traditional SSAO it uses a randomized Poisson disk kernel.
It assumes that neighbours with more vibrant (saturated) colors are occluding the center pixel - as this difference exceeds a threshold an occlusion is accounted.


How to use LBAO
---------------

1) Import the package in your project and attach LBAO script to your main camera.
2) Customize LBAO parameters like sample count, threshold and final blend. Each parameter shows a short tooltip describing the option.
3) Delete the demo folder if no longer needed.


Support
-------

* Support: contact@kronnect.me
* Website-Forum: http://kronnect.me
* Twitter: @KronnectGames


Future updates
--------------

All our assets follow an incremental development process by which a few beta releases are published on our support forum (kronnect.com).
We encourage you to signup and engage our forum. The forum is the primary support and feature discussions medium.

Of course, all updates of LBAO will be eventually available on the Asset Store.



Version history
---------------
Version 1.0.1 Current Release
- Improved performance
- [Fix] Fixed SM3.0 warning during build

Version 1.0 Sep-2017