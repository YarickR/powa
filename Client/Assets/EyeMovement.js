function Update () {
           
    /////////////////////
    //keyboard scrolling
   
    var translationX : float = Input.GetAxis("Horizontal");
    var translationY : float = Input.GetAxis("Vertical");
    var fastTranslationX : float = 2 * Input.GetAxis("Horizontal");
    var fastTranslationY : float = 2 * Input.GetAxis("Vertical");
   
    if (Input.GetKey(KeyCode.LeftShift))
        {
        transform.Translate(fastTranslationX + fastTranslationY, fastTranslationY - fastTranslationX, 0 );
        }
    else
        {
        transform.Translate(translationX + translationY, translationY - translationX, 0 );
        }
 
    ////////////////////
    //mouse scrolling
   
    var mousePosX = Input.mousePosition.x;
    var mousePosY = Input.mousePosition.y;
    var scrollDistance : int = 10 ;
    var scrollSpeed : float = 10;
 
    //Horizontal camera movement
/*
    if (mousePosX < scrollDistance)
        //horizontal, left
        {
        transform.Translate(-0.5, 0, 0);
        }
    if (mousePosX >= Screen.width - scrollDistance)
        //horizontal, right
        {
        transform.Translate(0.5, 0, 0);
        }
 
    //Vertical camera movement
    if (mousePosY < scrollDistance)
        //scrolling down
        {
        transform.Translate(0, 0, -0.5);
        }
    if (mousePosY >= Screen.height - scrollDistance)
        //scrolling up
        {
        transform.Translate(0, 0, 0.5);
        }
  */ 
    ////////////////////
    //zooming
    var Eye : GameObject = GameObject.Find("Eye");
    var camera: Camera =  Eye.GetComponent.<Camera>();
    //
    if ((Input.GetAxis("Mouse ScrollWheel") > 0) &&  (Eye.GetComponent.<Camera>().fieldOfView > 2))
        {
        	camera.fieldOfView -= 2;
        }
   
    //
    if ((Input.GetAxis("Mouse ScrollWheel") < 0) && (Eye.GetComponent.<Camera>().fieldOfView < 100))
        {
        	camera.fieldOfView  += 2;
        }
 
    //default zoom
    if (Input.GetKeyDown(KeyCode.Mouse2))
        {
          camera.fieldOfView = 50;
        }
     if (Input.touchCount == 2)
        {
            // Store both touches.
            var touchZero: Touch = Input.GetTouch(0);
            var touchOne: Touch = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            var touchZeroPrevPos : Vector2= touchZero.position - touchZero.deltaPosition;
            var touchOnePrevPos : Vector2 = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            var prevTouchDeltaMag : float= (touchZeroPrevPos - touchOnePrevPos).magnitude;
            var touchDeltaMag : float= (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            var deltaMagnitudeDiff: float = prevTouchDeltaMag - touchDeltaMag;

            // If the camera is orthographic...
            if (camera.orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.
                camera.orthographicSize += deltaMagnitudeDiff * 0.5f;

                // Make sure the orthographic size never drops below zero.
                camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.1f);
            }
            else
            {
                // Otherwise change the field of view based on the change in distance between the touches.
                camera.fieldOfView += deltaMagnitudeDiff * 0.5f;

                // Clamp the field of view to make sure it's between 0 and 180.
                camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 0.1f, 179.9f);
            }
        }  
}