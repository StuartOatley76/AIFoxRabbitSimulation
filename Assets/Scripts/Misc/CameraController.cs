
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CameraController : MonoBehaviour
{
    private Vector3 mouseMoveStart;
    private Camera thisCamera;

    [SerializeField]
    private float dragSpeed = 1f;

    [SerializeField]
    private float edgeScrollDistance = 30;

    [SerializeField]
    private float EdgeScrollSpeed = 1f;

    [SerializeField]
    private Renderer ground;

    private Bounds groundBounds;

    [SerializeField]
    private float maxYpos;

    [SerializeField]
    private float minYPos;

    [SerializeField]
    private float zoomSpeed;

    private void Awake() {
        thisCamera = GetComponent<Camera>();
        if (ground) {
            groundBounds = ground.bounds;
        }
    }
    private void LateUpdate () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ChangeScenes();
        }
        if (!thisCamera) {
            return;
        }
        HandleDrag();
        HandleEdgeScroll();
        HandleZoom();
        ClampPosition();

    }

    private void ChangeScenes() {
        BehaviourTreeObject[] trees = FindObjectsOfType<BehaviourTreeObject>();
        foreach(BehaviourTreeObject tree in trees) {
            tree.Running = false;
        }
        Node[] nodes = FindObjectsOfType<Node>();
        foreach(Node node in nodes) {
            node.Reset();
        }
        Menu.RunMenu();
    }

    private void HandleDrag() {
        if (Input.GetMouseButtonDown(0)) {
            mouseMoveStart = Input.mousePosition;
        }
        if (Input.GetMouseButton(0)) {
            Vector3 pos = thisCamera.ScreenToViewportPoint(Input.mousePosition - mouseMoveStart);
            Vector3 toMove = new Vector3(pos.x * dragSpeed * Time.deltaTime, 0, pos.y * dragSpeed * Time.deltaTime);
            thisCamera.transform.Translate(toMove, Space.World);
        }
    }

    private void HandleZoom() {
        if(Input.mouseScrollDelta.y == 0) {
            return;
        }
        float yPos = transform.position.y - ( Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed);
        yPos = Mathf.Clamp(yPos, minYPos, maxYpos);
        thisCamera.transform.position = new Vector3(thisCamera.transform.position.x, yPos, thisCamera.transform.position.z);

    }

    private void HandleEdgeScroll() {
        float distanceToTop = thisCamera.pixelHeight - Input.mousePosition.y;
        float distanceToBottom = Input.mousePosition.y;
        float distanceToRight = thisCamera.pixelWidth - Input.mousePosition.x;
        float distanceToLeft = Input.mousePosition.x;

        if (distanceToTop < edgeScrollDistance && distanceToTop > 0) {
            thisCamera.transform.position += Vector3.forward * Time.deltaTime * EdgeScrollSpeed * (edgeScrollDistance - distanceToTop);
        } else if (distanceToBottom < edgeScrollDistance && distanceToBottom > 0) {
            thisCamera.transform.position += Vector3.back * Time.deltaTime * EdgeScrollSpeed * (edgeScrollDistance - distanceToBottom);
        }
        if (distanceToLeft < edgeScrollDistance && distanceToLeft > 0) {
            thisCamera.transform.position += Vector3.left * Time.deltaTime * EdgeScrollSpeed * (edgeScrollDistance - distanceToLeft);
        } else if (distanceToRight < edgeScrollDistance && distanceToRight > 0) {
            thisCamera.transform.position += Vector3.right * Time.deltaTime * EdgeScrollSpeed * (edgeScrollDistance - distanceToRight);
        }
    }

    private void ClampPosition() {
        if (ground) {
            if (groundBounds.min.x > thisCamera.transform.position.x) {
                thisCamera.transform.position = new Vector3(groundBounds.min.x, thisCamera.transform.position.y, transform.position.z);
            }
            if (groundBounds.max.x < thisCamera.transform.position.x) {
                thisCamera.transform.position = new Vector3(groundBounds.max.x, thisCamera.transform.position.y, transform.position.z);
            }
            if (groundBounds.min.z > thisCamera.transform.position.z) {
                thisCamera.transform.position = new Vector3(transform.position.x, thisCamera.transform.position.y, groundBounds.min.z);
            }
            if (groundBounds.max.z < thisCamera.transform.position.z) {
                thisCamera.transform.position = new Vector3(transform.position.x, thisCamera.transform.position.y, groundBounds.max.z);
            }
        }
    }
}
