#include "glass.inc"
#include "metals.inc"

#macro MakeGlass(Glass_Color_R, Glass_Color_G, Glass_Color_B)
  material
  {
    texture
    {
      pigment { color rgbf <Glass_Color_R, Glass_Color_G, Glass_Color_B, 1> }
      finish { F_Glass6 }
    }
    interior
    {
      I_Glass_Exp(2)
      fade_color color rgb <0.35, 0.65, 0.85>
    }
  }
#end

//==================================================
//Material definitions
#declare Material_006_= MakeGlass(1, 0, 1)
//==================================================
#declare Material_004_= MakeGlass(0, .75, 1)
//==================================================
#declare Material_002_= MakeGlass(1, .75, 0)
//==================================================
#declare Material_= MakeGlass(1, 0, 0)
//==================================================
#declare Material_003_= MakeGlass(0, 1, 0)
//==================================================
#declare Material_005_= Material_004_
//==================================================
#declare Material_001_= MakeGlass(1, .5, 0)
//==================================================
#declare Material_007_= 
material
{
  texture
  {
    pigment { color rgb <0, 0, .75> }
    finish { F_MetalA }
  }
}