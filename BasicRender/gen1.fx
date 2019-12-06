struct intersectResult {
    float t0;
    float t1;  
    bool hit;
};

struct sphere {
    float3 Origin;
    float Radius;
    float4 SurfaceColor;
    float3 EmissionColor;
    float Reflection;
};

static float pi = 3.141592653589793f;

static sphere ss[6] = {
	{float3(0.0f, -10005.0f, -20.0f), 10000.0f, float4(0.2f, 0.2f, 0.2f, 1.0f), float3(0.0f, 0.0f, 0.0f), 0.0f},
	{float3(0.0f, 2.0f, -20.0f), 4.0f, float4(1.0f, 0.32f, 0.36f, 0.1f), float3(0.0f, 0.0f, 0.0f), 1.0f},
	{float3(5.0f, 0.0f, -15.0f), 2.0f, float4(0.9f, 0.76f, 0.46f, 0.1f), float3(0.0f, 0.0f, 0.0f), 1.0f},
	{float3(5.0f, 1.0f, -25.0f), 3.0f, float4(0.65f, 0.77f, 0.97f, 0.1f), float3(0.0f, 0.0f, 0.0f), 1.0f},
	{float3(-5.5f, 1.0f, -15.0f), 3.0f, float4(0.9f, 0.9f, 0.9f, 1.0f), float3(0.0f, 0.0f, 0.0f), 1.0f},
	{float3(20.0f, 30.0f, -40.0f), 3.0f, float4(0.0f, 0.0f, 0.0f, 1.0f), float3(3.0f, 3.0f, 3.0f), 0.0f}
};

intersectResult intersect(float3 rayorig, float3 raydir, sphere sp) {
	
	intersectResult res = {0.0f, 0.0f, false};
	
	float3 l = sp.Origin - rayorig;
	float tca = dot(l, raydir);
	if(tca < 0.0f) return res;
	
	float d2 = dot(l, l) - tca * tca;
	float r2 = sp.Radius * sp.Radius;
	if(d2 > r2) return res;
	
	float thc = sqrt(r2 - d2);
	res.t0 = tca - thc;
	res.t1 = tca + thc;
	res.hit = true;
	
	return res;
}

float3 trace3(float3 rayorig, float3 raydir) {
	
	float tnear = 100000000.0f;
	sphere sp;
	bool hit = false;
	for(int i = 0; i < 6; i++) {
		
		intersectResult iRes = intersect(rayorig, raydir, ss[i]);
		if(iRes.hit) {
			if(iRes.t0 < 0.0f) iRes.t0 = iRes.t1;
			if(iRes.t0 < tnear) {
				tnear = iRes.t0;
				sp = ss[i];
				hit = true;
			}
		}
	}
	
	if(!hit) {
		return float3(1.0f, 1.0f, 1.0f);
	}
	
	float3 surfaceColor = float3(0.0f, 0.0f, 0.0f);
	float3 phit = rayorig + raydir * tnear;
	float3 nhit = normalize(phit - sp.Origin);
	
	float bias = 0.0001f;
	bool inside = false;
	if(dot(raydir, nhit) > 0) {
		nhit = -1.0f * nhit;
		inside = true;
	}
		
	for(int k = 0; k < 6; k++) {
		if(ss[k].EmissionColor.x > 0.0f) {
		
			float3 transmission = float3(1.0f, 1.0f, 1.0f);
			float3 lightDirection = normalize(ss[k].Origin - phit);
			
			for(int l = 0; l < 6; l++) {
				if(k != l) {
					intersectResult iResL = intersect(phit + nhit * bias, lightDirection, ss[l]);
					if(iResL.hit) {
						transmission = float3(0.0f, 0.0f, 0.0f);
						break;
					}
				}
			}
			
			surfaceColor += float3(sp.SurfaceColor.x, sp.SurfaceColor.y, sp.SurfaceColor.z) * transmission * (0.0f > dot(nhit, lightDirection) ? 0.0f : dot(nhit, lightDirection)) * ss[k].EmissionColor;
		}
	}
	
	
	return surfaceColor + sp.EmissionColor;
}

float3 trace2(float3 rayorig, float3 raydir) {
	
	float tnear = 100000000.0f;
	sphere sp;
	bool hit = false;
	for(int i = 0; i < 6; i++) {
		
		intersectResult iRes = intersect(rayorig, raydir, ss[i]);
		if(iRes.hit) {
			if(iRes.t0 < 0.0f) iRes.t0 = iRes.t1;
			if(iRes.t0 < tnear) {
				tnear = iRes.t0;
				sp = ss[i];
				hit = true;
			}
		}
	}
	
	if(!hit) {
		return float3(1.0f, 1.0f, 1.0f);
	}
	
	float3 surfaceColor = float3(0.0f, 0.0f, 0.0f);
	float3 phit = rayorig + raydir * tnear;
	float3 nhit = normalize(phit - sp.Origin);
	
	float bias = 0.0001f;
	bool inside = false;
	if(dot(raydir, nhit) > 0) {
		nhit = -1.0f * nhit;
		inside = true;
	}
		
	if(sp.SurfaceColor.a <= 1.0f || sp.Reflection > 0.0f) {
			
		float facingratio = -dot(raydir, nhit);
		float frensel = lerp(pow(1 - facingratio, 3.0f), 1.0f, 0.1f);
		float3 refldir = normalize(reflect(raydir, nhit));
		float3 spreflection = trace3(phit + nhit * bias, refldir);
		float3 sprefraction = float3(0.0f, 0.0f, 0.0f);
		
		if(sp.SurfaceColor.a != 1.0f) {
			
			float ior = 1.1f;
			float eta = inside ? ior : 1.0f / ior;
			float cosi = -dot(nhit, raydir);
			float k = 1.0f - eta * eta * (1.0f - cosi * cosi);
			float3 refrdir = normalize(raydir * eta + nhit * (eta * cosi - sqrt(k)));
			sprefraction = trace3(phit - nhit * bias, refrdir);
		}
			
		surfaceColor = (spreflection * frensel + sprefraction * (1.0f - frensel) * (1.0f - sp.SurfaceColor.a)) * sp.SurfaceColor;
	}
	else {
		for(int k = 0; k < 6; k++) {
			if(ss[k].EmissionColor.x > 0.0f) {
			
				float3 transmission = float3(1.0f, 1.0f, 1.0f);
				float3 lightDirection = normalize(ss[k].Origin - phit);
				
				for(int l = 0; l < 6; l++) {
					if(k != l) {
						intersectResult iResL = intersect(phit + nhit * bias, lightDirection, ss[l]);
						if(iResL.hit) {
							transmission = float3(0.0f, 0.0f, 0.0f);
							break;
						}
					}
				}
				
				surfaceColor += float3(sp.SurfaceColor.x, sp.SurfaceColor.y, sp.SurfaceColor.z) * transmission * (0.0f > dot(nhit, lightDirection) ? 0.0f : dot(nhit, lightDirection)) * ss[k].EmissionColor;
			}
		}
	}
	
	return surfaceColor + sp.EmissionColor;
}

float3 trace1(float3 rayorig, float3 raydir) {
	
	float tnear = 100000000.0f;
	sphere sp;
	bool hit = false;
	for(int i = 0; i < 6; i++) {
		
		intersectResult iRes = intersect(rayorig, raydir, ss[i]);
		if(iRes.hit) {
			if(iRes.t0 < 0.0f) iRes.t0 = iRes.t1;
			if(iRes.t0 < tnear) {
				tnear = iRes.t0;
				sp = ss[i];
				hit = true;
			}
		}
	}
	
	if(!hit) {
		return float3(1.0f, 1.0f, 1.0f);
	}
	
	float3 surfaceColor = float3(0.0f, 0.0f, 0.0f);
	float3 phit = rayorig + raydir * tnear;
	float3 nhit = normalize(phit - sp.Origin);
	
	float bias = 0.0001f;
	bool inside = false;
	if(dot(raydir, nhit) > 0.0f) {
		nhit = -1.0f * nhit;
		inside = true;
	}
		
	if(sp.SurfaceColor.a <= 1.0f || sp.Reflection > 0.0f) {
			
		float facingratio = -dot(raydir, nhit);
		float frensel = lerp(pow(1 - facingratio, 3.0f), 1.0f, 0.1f);
		float3 refldir = normalize(reflect(raydir, nhit));
		float3 spreflection = trace2(phit + nhit * bias, refldir);
		float3 sprefraction = float3(0.0f, 0.0f, 0.0f);
		
		if(sp.SurfaceColor.a != 1.0f) {
			
			float ior = 1.1f;
			float eta = inside ? ior : 1.0f / ior;
			float cosi = -dot(nhit, raydir);
			float k = 1.0f - eta * eta * (1.0f - cosi * cosi);
			float3 refrdir = normalize(raydir * eta + nhit * (eta * cosi - sqrt(k)));
			sprefraction = trace2(phit - nhit * bias, refrdir);
		}
			
		surfaceColor = (spreflection * frensel + sprefraction * (1.0f - frensel) * (1.0f - sp.SurfaceColor.a)) * sp.SurfaceColor;
	}
	else {
		for(int k = 0; k < 6; k++) {
			if(ss[k].EmissionColor.x > 0.0f) {
			
				float3 transmission = float3(1.0f, 1.0f, 1.0f);
				float3 lightDirection = normalize(ss[k].Origin - phit);
				
				for(int l = 0; l < 6; l++) {
					if(k != l) {
						intersectResult iResL = intersect(phit + nhit * bias, lightDirection, ss[l]);
						if(iResL.hit) {
							transmission = float3(0.0f, 0.0f, 0.0f);
							break;
						}
					}
				}
				
				surfaceColor += float3(sp.SurfaceColor.x, sp.SurfaceColor.y, sp.SurfaceColor.z) * transmission * (0.0f > dot(nhit, lightDirection) ? 0.0f : dot(nhit, lightDirection)) * ss[k].EmissionColor;
			}
		}
	}
	
	return surfaceColor + sp.EmissionColor;
}

float2 screenResolution : register(c0);
float fov : register(c1);
float3 cameraOrigin : register(c2);
float3 cameraAngle: register(c3);

float3 sphereOrigin1: register(c4);
float sphereRadius1: register(c5);

float3 sphereOrigin2: register(c6);
float sphereRadius2: register(c7);

float3 sphereOrigin3: register(c8);
float sphereRadius3: register(c9);

float3 sphereOrigin4: register(c10);
float sphereRadius4: register(c11);

float4 main(float2 uv : TEXCOORD) : COLOR {

	float aspectRatio = screenResolution.x / screenResolution.y;
	
	float tga = tan(fov * 0.5f * pi/180.0f);
	
	float xCam = tga * (uv.x * 2.0f - 1.0f) * aspectRatio;
	float yCam = tga * (1.0f - uv.y * 2.0f);
	
	ss[1].Origin = sphereOrigin1;
	ss[1].Radius = sphereRadius1;
	
	ss[2].Origin = sphereOrigin2;
	ss[2].Radius = sphereRadius2;
	
	ss[3].Origin = sphereOrigin3;
	ss[3].Radius = sphereRadius3;
	
	ss[4].Origin = sphereOrigin4;
	ss[4].Radius = sphereRadius4;
	
	float3 cameraDirection = normalize(float3(xCam, yCam, -1.0f));
	
	return float4(trace1(cameraOrigin, cameraDirection), 1.0f);
}