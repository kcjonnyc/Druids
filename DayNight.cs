using UnityEngine;
using System.Collections;

public class DayNight : MonoBehaviour {

	// variables are public for access in unity editor
	// gradients are used as color changes as time changes
	public Gradient dayNightColor;
	public Gradient fogColor;

	// curve is used to adjust the fog density as time changes
	public AnimationCurve fogDensityCurve;

	// intensity values
	public float maxIntensity = 3f;
	public float minIntensity = 0f;

	// minPoint indicates the point around the horizon where
	// the sun is considered set/gone
	public float minPoint = -0.2f;

	// ambient light values
	public float maxAmbient = 1f;
	public float minAmbient = 0f;
	public float minAmbientPoint = -0.2f;

	// amount of fog used
	public float fogScale = 1f;

	// atmosphere thickness is used to account for colour changes
	// for example, night sky may be too red, so atmosphere increases to cover it up
	public float dayAtmosphereThickness = 0.4f;
	public float nightAtmosphereThickness = 0.87f;

	// day and night rotation speeds
	public Vector3 dayRotateSpeed;
	public Vector3 nightRotateSpeed;

	// additional objects that need to be used for adjustment
	Light mainLight;
	Skybox sky;
	Material skyMaterial;

	void Start () {
		// gets a reference to the Light object the script is attached to
		mainLight = GetComponent<Light> ();
		skyMaterial = RenderSettings.skybox;
	}

	void Update () {
		// intensity calculation for main light
		// range is the time the day takes up (1 being the sun at it's highest point
		float range = 1 - minPoint;
		// the dot product between the direction of the main light and the standard vector pointing down is
		// calculated (the value dot will contain a value between 1 and -1) 
		float dot = Mathf.Clamp01 ((Vector3.Dot (mainLight.transform.forward, Vector3.down) - minPoint) / range);
		// caluclation for intensity
		float intensity = ((maxIntensity - minIntensity) * dot) + minIntensity;
		// mainlight intesnsity is set
		mainLight.intensity = intensity;

		// the same caluclation is repeated for ambient light
		range = 1 - minAmbientPoint;
		dot = Mathf.Clamp01 ((Vector3.Dot (mainLight.transform.forward, Vector3.down) - minAmbientPoint) / range);
		intensity = ((maxAmbient - minAmbient) * dot) + minAmbient;
		// ambient light is set through the render settings
		RenderSettings.ambientIntensity = intensity;

		// dot product is used to calculate main light colour and ambient light colour
		// the evaluate method within a gradient can be called to calculate colour
		// dot will be a value between 0 and 1 after Mathf.Clamp01
		mainLight.color = dayNightColor.Evaluate(dot);
		// ambient light is set to the same colour as the main light
		RenderSettings.ambientLight = mainLight.color;

		// a similar calculation will be used for fog
		RenderSettings.fogColor = dayNightColor.Evaluate(dot);
		RenderSettings.fogDensity = fogDensityCurve.Evaluate(dot) * fogScale;

		// intensity is now determined for atmosphere
		intensity = ((dayAtmosphereThickness - nightAtmosphereThickness) * dot) + nightAtmosphereThickness;
		// sets a property within our created skybox
		skyMaterial.SetFloat ("_AtmosphereThickness", intensity);

		// if sun is up, it is day time, the day rotate speed will be used
		// if son is down, dot product is negative, night rotate speed will be used for rotation
		if (dot > 0)
			transform.Rotate (dayRotateSpeed * Time.deltaTime);
		else
			transform.Rotate (nightRotateSpeed * Time.deltaTime);
	}
}
