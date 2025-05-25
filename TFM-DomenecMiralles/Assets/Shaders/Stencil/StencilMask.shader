Shader "Custom/StencilMask"
{
    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry-1" }

        Pass
        {
            Stencil
            {
                Ref 1
                Comp always
                Pass replace
            }

            ColorMask 0 // No rendering of colors
            ZWrite Off  // No writing to the depth buffer
        }
    }
}