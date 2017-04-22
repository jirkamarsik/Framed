namespace GameCode
 
open UnityEngine

type PaintballShooter() = 
    inherit MonoBehaviour()
 
    [<SerializeField>]
    let mutable paintballPrefab : GameObject = null

    member this.Update() =
        if Input.GetMouseButtonDown(0) then
            let newBall = Object.Instantiate(paintballPrefab, this.transform.position, this.transform.rotation) :?> GameObject
            newBall.GetComponent<Rigidbody>().velocity <- new Vector3(0.0f, 5.0f, 0.0f)