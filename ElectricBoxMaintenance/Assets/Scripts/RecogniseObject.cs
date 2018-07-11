using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class RecogniseObject : MonoBehaviour, IInputClickHandler {

    [SerializeField]
    public GameObject _object;
    string debugInfo;
    bool scanning = false;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("Requesting scan finish");
        SpatialUnderstanding.Instance.RequestFinishScan();
        eventData.Use();
    }

    public void ScanStateChanged()
    {
        switch (SpatialUnderstanding.Instance.ScanState)
        {
            case SpatialUnderstanding.ScanStates.Scanning:
                Debug.Log("start scan");
                LogSurfaceState();
                break;
            case SpatialUnderstanding.ScanStates.Done:
                Debug.Log("scan finished");
                LogSurfaceState();
                InstantiateObjectOnTable();
                break;
            default:
                break;
        }
    }

    private void LogSurfaceState()
    {
        IntPtr statPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
        if (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statPtr) != 0)
        {
            var stats = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStats();
            string debugInfo = String.Format("TotalSurfaceArea: {0:0.##}\n" +
                                      "WallSurfaceArea: {1:0.##}\n" +
                                      "HorizSurfaceArea: {2:0.##}",
                                      stats.TotalSurfaceArea,
                                      stats.WallSurfaceArea,
                                      stats.HorizSurfaceArea);
            Debug.Log(debugInfo);
        }
    }

    void AddShapeDefinition(string shapeName,
                        List<SpatialUnderstandingDllShapes.ShapeComponent> shapeComponent,
                        List<SpatialUnderstandingDllShapes.ShapeConstraint> shapeConstraint)
    {
        IntPtr shapeComponentsPtr = (shapeComponent == null) ? IntPtr.Zero : SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(shapeComponent.ToArray());
        IntPtr shapeConstraintsPtr = (shapeConstraint == null) ? IntPtr.Zero : SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(shapeConstraint.ToArray());
        if (SpatialUnderstandingDllShapes.AddShape(shapeName,
            (shapeComponent == null) ? 0 : shapeComponent.Count,
            shapeComponentsPtr,
            (shapeConstraint == null) ? 0 : shapeConstraint.Count,
            shapeConstraintsPtr) == 0)
        {
            Debug.Log("Failed to create object");
        }
    }

    private void CreateElectricBoxShape()
    {
        var shapeComponents = new List<SpatialUnderstandingDllShapes.ShapeComponent>()
        {
            new SpatialUnderstandingDllShapes.ShapeComponent(
                new List<SpatialUnderstandingDllShapes.ShapeComponentConstraint>()
                {
                    SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_RectangleWidth_Between(0.3f, 0.5f),
                    SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_RectangleLength_Between(0.4f, 0.6f),
                    SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_SurfaceCount_Is(5),
                    SpatialUnderstandingDllShapes.ShapeComponentConstraint.Create_IsRectangle(),
                }),
        };

        var shapeConstraints = new List<SpatialUnderstandingDllShapes.ShapeConstraint>()
        {
            SpatialUnderstandingDllShapes.ShapeConstraint.Create_NoOtherSurface(),
        };

        AddShapeDefinition("ElectricBox", shapeComponents, shapeConstraints);
        SpatialUnderstandingDllShapes.ActivateShapeAnalysis();
    }

    private void InstantiateObjectOnTable()
    {
        const int MaxResultCount = 512;
        var shapeResults = new SpatialUnderstandingDllShapes.ShapeResult[MaxResultCount];

        var resultsShapePtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(shapeResults);
        var locationCount = SpatialUnderstandingDllShapes.QueryShape_FindPositionsOnShape("ElectricBox", 0.1f, shapeResults.Length, resultsShapePtr);

        if (locationCount > 0)
        {
            Instantiate(_object,
                        shapeResults[0].position,
                        Quaternion.LookRotation(shapeResults[0].position.normalized, Vector3.up));
            // For some reason the halfDims of the shape result are always 0,0,0 so we can't scale 
            // to the size of the surface. This may be a bug in the HoloToolkit?
            Debug.Log("Placed Hologram");
        }
        else
        {
            // Create a fallback - Instantiate the cube in front of the player and let the user tap
            // it and place it where ever the fuck they want (includes walls)
            Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 2f;

            var cube = Instantiate(_object, pos, Quaternion.identity);
            cube.AddComponent<HoloToolkit.Unity.Interpolator>();
            cube.AddComponent<HoloToolkit.Unity.SpatialMapping.TapToPlace>();
            Debug.Log("Not enough space for the hologram");
        }
    }


    // Use this for initialization
    void Start () {
        InputManager.Instance.PushModalInputHandler(gameObject);
        SpatialUnderstanding.Instance.ScanStateChanged += ScanStateChanged;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
