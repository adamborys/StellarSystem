﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StellarSystem {

	public List<OrbitProvider> Orbits;
	public List<Transform> PlanetTransforms;
	public List<Planet> Planets;
	public int GameDuration = 900;

	public StellarSystem(SystemCreator creator, GameObject origin) {
		this.Orbits = new List<OrbitProvider>();
		this.Planets = new List<Planet>();
		this.PlanetTransforms = new List<Transform>();

		System.Random randomizer = new System.Random();
		for(int i = 0; i < creator.PlanetQuantity; i++) {
			Planets.Add(new Planet(i, randomizer));

			this.Orbits.Add(creator.Orbits[i].GetComponent<OrbitProvider>());

			GameObject planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			planet.name = "Planet";
			planet.transform.localScale = new Vector3(this.Planets[i].Scale,this.Planets[i].Scale,this.Planets[i].Scale);

			this.PlanetTransforms.Add(planet.transform);
			this.PlanetTransforms[i].parent = creator.Orbits[i].transform;
		}
	}

	public float GetDistanceBetween(Transform firstPlanet, Transform secondPlanet) {
		return Vector3.Distance(firstPlanet.position, secondPlanet.position);
	}
}
