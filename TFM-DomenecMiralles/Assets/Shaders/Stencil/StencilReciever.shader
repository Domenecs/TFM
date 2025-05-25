Shader "Custom/StencilReciever"
{
    SubShader
    {
            Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        Stencil
        {
            Ref 1
            Comp NotEqual
            Pass keep
        }

        Pass
        {

        }
    }
}
