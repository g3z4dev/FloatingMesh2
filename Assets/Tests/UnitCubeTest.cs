using NUnit.Framework;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using FloatingMesh2;
using System;

public class UnitCubeTest
{
    [SetUp]
    public void SetUp()
    {
        EditorSceneManager.OpenScene("Assets/TestScenes/UnitCubeScene.unity", OpenSceneMode.Single);
    }

    [Test]
    public void UnitCubeTestSimple()
    {
        GameObject cube = GameObject.Find("Cube");
        MeshFilter mesh = cube.GetComponent<MeshFilter>();
        Rigidbody body = cube.GetComponent<Rigidbody>();
        SimulationScene scene = new();
        scene.fluid.fluidLevel = 0;
        scene.fluid.fluidDensity = 1;
        scene.world.gravity = Physics.gravity.magnitude;
        BuoyantForceCalculator calculator = new ExactSurfaceBFC(mesh.sharedMesh, cube.transform, scene);
        calculator.Calculate(out _, out Vector3 buoyantForce);
        Assert.LessOrEqual(Math.Abs(buoyantForce.magnitude - scene.fluid.GetHydrostaticPressure(new Vector3(0, -0.5f, 0))), 1e-8);
    }
}
