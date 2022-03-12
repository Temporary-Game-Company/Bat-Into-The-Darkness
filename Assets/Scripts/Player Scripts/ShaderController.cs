using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace McgillTeam3
{
    public class ShaderController : MonoBehaviour
    {
        [SerializeField] private Material wallMaterial;
        [SerializeField] private GameObject player;
        [SerializeField] private float rippleMaxDistance = 10f;
        [SerializeField] private float rippleDuration = 0.2f;
        [SerializeField] private float fadeDuration = 0.15f;
        [SerializeField] private string rippleCenter= "_Ripple_Center";
        [SerializeField] private string rippleDistance = "_Ripple_Distance";
        [SerializeField] private string fadeAmount = "_Fade_Amount";

        private bool _echolocating;

        private Echolocation _echolocation;

        float time = 0;
        float rippleStartValue;
        float fadeStartValue;

        private void Awake()
        {
            _echolocation = player.GetComponent<Echolocation>();
        }

        private void Start()
        {
            rippleStartValue = wallMaterial.GetFloat(rippleDistance);
            fadeStartValue = wallMaterial.GetFloat(fadeAmount);
        }

        private void OnEnable()
        {
            Echolocation.OnStartEcholocate += OnStartEcholocate;
            Echolocation.OnEndEcholocate += OnEndEcholocate;
        }

        private void OnDisable()
        {
            Echolocation.OnStartEcholocate -= OnStartEcholocate;
            Echolocation.OnEndEcholocate -= OnEndEcholocate;
        }

        private void OnStartEcholocate()
        {
            time = 0;
            wallMaterial.SetFloat(fadeAmount, 0);
            rippleStartValue = wallMaterial.GetFloat(rippleDistance);
            _echolocating = true;
        }

        private void OnEndEcholocate()
        {
            time = 0;
            fadeStartValue = wallMaterial.GetFloat(fadeAmount);
           _echolocating = false;
        }
        
        private void Update()
        {
            wallMaterial.SetVector(rippleCenter, player.transform.position);
            // print(_echolocating);
            // print(time);

            if (_echolocating && time < rippleDuration){
                wallMaterial.SetFloat(rippleDistance, Mathf.Lerp(rippleStartValue, rippleMaxDistance, time / rippleDuration));
                time += Time.deltaTime;
            }
            else if (_echolocating && time >= rippleDuration){
                wallMaterial.SetFloat(rippleDistance, rippleMaxDistance);
            }
            else if (!_echolocating && time < fadeDuration)
            {
                wallMaterial.SetFloat(fadeAmount, Mathf.Lerp(fadeStartValue, 1, time / fadeDuration));
                time += Time.deltaTime;
            }
            else if (!_echolocating && time >= fadeDuration)
            {
                wallMaterial.SetFloat(fadeAmount, 1f);
                wallMaterial.SetFloat(rippleDistance, 0);
            }
        }
    }
}