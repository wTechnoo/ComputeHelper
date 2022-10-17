#ifndef COMPUTE_CG_HELPER
#define COMPUTE_CG_HELPER

float4 getPositionFromMatrix(float4x4 mat, float4 vertex){
	return mul(mat, vertex);
}

float4x4 compose(float3 position)  
{  
    float4x4 m = float4x4(float4(0, 0, 0, 0), float4(0, 0, 0, 0), float4(0, 0, 0, 0), float4(0, 0, 0, 0));  
  
    float x = 0, y = 0, z = 0, w = 1;  
    float x2 = x + x, y2 = y + y, z2 = z + z;  
    float xx = x * x2, xy = x * y2, xz = x * z2;  
    float yy = y * y2, yz = y * z2, zz = z * z2;  
    float wx = w * x2, wy = w * y2, wz = w * z2;  
  
    m[0][0] = 1.0 - (yy + zz);  
    m[0][1] = xy - wz;  
    m[0][2] = xz + wy;  
  
    m[1][0] = xy + wz;  
    m[1][1] = 1.0 - (xx + zz);  
    m[1][2] = yz - wx;  
  
    m[2][0] = xz - wy;  
    m[2][1] = yz + wx;  
    m[2][2] = 1.0 - (xx + yy);  
  
    m[3][3] = 1.0;  
  
    float sx = 1, sy = 1, sz = 1;  
  
    m[0][0] *= sx; m[1][0] *= sy; m[2][0] *= sz;  
    m[0][1] *= sx; m[1][1] *= sy; m[2][1] *= sz;  
    m[0][2] *= sx; m[1][2] *= sy; m[2][2] *= sz;  
    m[0][3] *= sx; m[1][3] *= sy; m[2][3] *= sz;  
  
    float tx = position.x, ty = position.y, tz = position.z;  
    m[0][3] = tx;  
    m[1][3] = ty;  
    m[2][3] = tz;  
  
    return m;  
}

float4x4 compose(float3 position, float3 scale)  
{  
    float4x4 m = float4x4(float4(0, 0, 0, 0), float4(0, 0, 0, 0), float4(0, 0, 0, 0), float4(0, 0, 0, 0));  
  
    float x = 0, y = 0, z = 0, w = 1;  
    float x2 = x + x, y2 = y + y, z2 = z + z;  
    float xx = x * x2, xy = x * y2, xz = x * z2;  
    float yy = y * y2, yz = y * z2, zz = z * z2;  
    float wx = w * x2, wy = w * y2, wz = w * z2;  
  
    m[0][0] = 1.0 - (yy + zz);  
    m[0][1] = xy - wz;  
    m[0][2] = xz + wy;  
  
    m[1][0] = xy + wz;  
    m[1][1] = 1.0 - (xx + zz);  
    m[1][2] = yz - wx;  
  
    m[2][0] = xz - wy;  
    m[2][1] = yz + wx;  
    m[2][2] = 1.0 - (xx + yy);  
  
    m[3][3] = 1.0;  
  
    float sx = scale.x, sy = scale.y, sz = scale.z;  
  
    m[0][0] *= sx; m[1][0] *= sy; m[2][0] *= sz;  
    m[0][1] *= sx; m[1][1] *= sy; m[2][1] *= sz;  
    m[0][2] *= sx; m[1][2] *= sy; m[2][2] *= sz;  
    m[0][3] *= sx; m[1][3] *= sy; m[2][3] *= sz;  
  
    float tx = position.x, ty = position.y, tz = position.z;  
    m[0][3] = tx;  
    m[1][3] = ty;  
    m[2][3] = tz;  
  
    return m;  
}

float4x4 compose(float3 position, float4 quat, float3 scale)  
{  
    float4x4 m = float4x4(float4(0, 0, 0, 0), float4(0, 0, 0, 0), float4(0, 0, 0, 0), float4(0, 0, 0, 0));  
  
    float x = quat.x, y = quat.y, z = quat.z, w = quat.w;  
    float x2 = x + x, y2 = y + y, z2 = z + z;  
    float xx = x * x2, xy = x * y2, xz = x * z2;  
    float yy = y * y2, yz = y * z2, zz = z * z2;  
    float wx = w * x2, wy = w * y2, wz = w * z2;  
  
    m[0][0] = 1.0 - (yy + zz);  
    m[0][1] = xy - wz;  
    m[0][2] = xz + wy;  
  
    m[1][0] = xy + wz;  
    m[1][1] = 1.0 - (xx + zz);  
    m[1][2] = yz - wx;  
  
    m[2][0] = xz - wy;  
    m[2][1] = yz + wx;  
    m[2][2] = 1.0 - (xx + yy);  
  
    m[3][3] = 1.0;  
  
    float sx = scale.x, sy = scale.y, sz = scale.z;  
  
    m[0][0] *= sx; m[1][0] *= sy; m[2][0] *= sz;  
    m[0][1] *= sx; m[1][1] *= sy; m[2][1] *= sz;  
    m[0][2] *= sx; m[1][2] *= sy; m[2][2] *= sz;  
    m[0][3] *= sx; m[1][3] *= sy; m[2][3] *= sz;  
  
    float tx = position.x, ty = position.y, tz = position.z;  
    m[0][3] = tx;  
    m[1][3] = ty;  
    m[2][3] = tz;  
  
    return m;  
}

#endif