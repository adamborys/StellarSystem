﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour {

	[Range(0,100)]
	public StellarSystem System;
	public float GameSpeed = 1;
	public float StartTime;
	public float LastUpdate;
	
	private GameObject systemOrigin;
	private float now;
  private const float timeQuantum = 0.025f;


	void Start () {
		systemOrigin = GameObject.Find("SystemOrigin");
	  SystemCreator systemCreator = systemOrigin.GetComponent<SystemCreator>();

		System = new StellarSystem(systemCreator, systemOrigin);
		
		for(int i = 0; i < systemCreator.PlanetQuantity; i++) {
			if(System.Planets[i].OrbitalPeriod < 0.01f) {
				System.Planets[i].OrbitalPeriod = 0.01f;
			}
			SetPosition(i, System.Planets[i].OrbitalProgress);
		}
		StartTime = Time.time;
		LastUpdate = Time.time;
	}

	void Update () {
		if(!StellarSystem.IsPaused && (now = Time.time) - LastUpdate >= timeQuantum) {
			UpdatePlanetPositions(now);
			LastUpdate = Time.time;
		}
	}

	public Vector3 GetPosition(int index, float progress) {
		Vector2 position = System.Orbits[index].OrbitShape.Evaluate(progress);
		return new Vector3(position.x, 0f, position.y);
	}

	public void SetPosition(int index, float progress) {
		Vector2 position = System.Orbits[index].OrbitShape.Evaluate(progress);
		System.PlanetTransforms[index].localPosition = new Vector3(position.x, 0f, position.y);
	}
	
	public void UpdatePlanetPositions(float targetTime) {
		for(int i = 0; i < System.Planets.Count; i++)
			for(float time = LastUpdate; time <= targetTime; time += timeQuantum) {
				float distanceFromStar = Vector3.Distance(System.PlanetTransforms[i].position, new Vector3());
				float orbitalSpeed = (GameSpeed/100) * ((i+1) / (System.Planets[i].OrbitalPeriod * distanceFromStar));
				System.Planets[i].OrbitalProgress += timeQuantum * orbitalSpeed;
				System.Planets[i].OrbitalProgress %= 1f;
				SetPosition(i, System.Planets[i].OrbitalProgress);
			}
	}
}