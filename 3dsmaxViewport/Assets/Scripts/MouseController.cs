using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Runtime;
using System.Diagnostics;

public class MouseController : MonoBehaviour {

    [DllImport("User32.Dll")]
    public static extern long SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("User32.Dll")]
    public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }


    private Process UnityProcess;
    public Vector2 MousePosition;
    public Vector2 RelativeMousePosition;
    public int Width;
    public int Height;
    public UILabel mosposlabel;

    // Use this for initialization
    void Start () {
         UnityProcess = Process.GetCurrentProcess();        
    }
	
	// Update is called once per frame
	void Update () {

        MousePosition = GetCursorPosition();
        Width = UnityEngine.Screen.width;
        Height = UnityEngine.Screen.height;
    }

    Vector2 ToRelativePosition(Vector2 pos)
    {
        POINT p = new POINT();
        p.x = Convert.ToInt32(pos.x);
        p.y = Convert.ToInt32(pos.y);
        ClientToScreen(UnityProcess.Handle, ref p);
        return pos;
    }
    void SetMousePosition(Vector2 pos)
    {
        POINT p = new POINT();
        p.x = Convert.ToInt32(pos.x);
        p.y = Convert.ToInt32(pos.y);
        SetCursorPos(p.x, p.y);
    }
    Vector2 GetCursorPosition()
    {
        POINT lpPoint;
        GetCursorPos(out lpPoint);
        Vector3 vec3 = Input.mousePosition;
        Vector2 vec = new Vector2(vec3.x - (UnityEngine.Screen.width / 2), vec3.y - (UnityEngine.Screen.height / 2));
        return vec;
    }
    Vector2 MouseToScreenPosition(Vector2 pos)
    {
        Vector3 vec = new Vector3(pos.x, pos.y, 0);
        Vector3 newvec = Camera.main.ViewportToScreenPoint(vec);
        return newvec;
    }



}
