// PoVRay 3.7 Scene File "logo.pov"
// author:  Joseph Dillon
// date:    May 18, 2012
//--------------------------------------------------------------------------
#version 3.7;
//--------------------------------------------------------------------------
#include "colors.inc"
#include "metals.inc"
#include "geometry.inc"
#include "rad_def.inc"

global_settings
{
  assumed_gamma 1.0
  max_trace_level 25
  photons {
    spacing 0.03*.32
    autostop 0
    jitter .4
    radius .15*.32
  }
  radiosity
  {
    Rad_Settings(Radiosity_Final, on, on)
  }
}

#default
{
  finish
  {
    ambient 0.1 diffuse 0.9
  }
}
//--------------------------------------------------------------------------
// camera ------------------------------------------------------------------
camera { perspective angle 75              // front view
         location  <-1.5 , 4.0 , -4.0>
         right     x*image_width/image_height
         look_at   <0.0 , 1.0 , 0.0> }
// lights -------------------------------------------------------------------
light_source
{
  <2, 2, -4> 
  color White

  photons {
    reflection on
    refraction on
  }
}
light_source
{
  <-2, 2, -4> 
  color White

  photons {
    reflection on
    refraction on
  }
}
light_source
{
  <-1, 4, 0> 
  color White*2

  photons {
    reflection on
    refraction on
  }
}
//---------------------------------------------------------------------------
//---------------------------- objects in scene -----------------------------
//---------------------------------------------------------------------------

object
{
  tetris_logo
  scale .5 rotate <90, 0, 0> translate <0, 1, -1.25>
}

box
{
  <-10, -2, -10>, <10, -.6, 10>
  texture
  {
    pigment { color White }
    finish { F_MetalA }
  }
}