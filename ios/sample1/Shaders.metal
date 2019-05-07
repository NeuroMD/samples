//
//  Shaders.metal
//  sample1
//
//  Created by admin on 22/04/2019.
//  Copyright © 2019 admin. All rights reserved.
//

#include <metal_stdlib>
using namespace metal;

struct VertexOut {
    float4 computedPosition [[position]];
};

vertex VertexOut basic_vertex(                           // 1
                           const device packed_float2* vertex_array [[ buffer(0) ]], // 2
                           constant float2& currenttimestamp [[ buffer(1) ]],
                           unsigned int vid [[ vertex_id ]]) {                 // 3
    
    VertexOut outVertex = VertexOut();
    outVertex.computedPosition = float4(vertex_array[vid][0]-currenttimestamp[0]/(1.5), vertex_array[vid][1]*currenttimestamp[1], 0.0, 1.0);
    return outVertex;
    //return float4(vertex_array[vid][0]-currenttimestamp[0]/(1.5), vertex_array[vid][1]*currenttimestamp[1], 0.0, 1.0);              // 4
}

fragment float4 basic_fragment(VertexOut interpolated [[stage_in]]) { // 1
    return float4(0.23, 0.54, 0.68, 1.0);              // 2
}
