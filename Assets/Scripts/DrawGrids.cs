/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2013.                                   *
* Leap Motion proprietary and  confidential.  Not for distribution.            *
* Use subject to the terms of the Leap Motion SDK Agreement available at       *
* https://developer.leapmotion.com/sdk_agreement, or another agreement between *
* Leap Motion and you, your company or other organization.                     *
\******************************************************************************/

using UnityEngine;
using System.Collections;

public class DrawGrids : MonoBehaviour {
	
	static Material s_LineMaterial = null;
	public float m_GridWidth = 1;
	public int m_GridDimensions = 100;
	public Color m_GridColor = Color.gray;
	
	
	static void CreateLineMaterial() {
    	if( !s_LineMaterial ) {
        	s_LineMaterial = new Material( "Shader \"Lines/Colored Blended\" {" +
           		"SubShader { Pass { " +
           		"    Blend SrcAlpha OneMinusSrcAlpha " +
            	"    ZWrite Off Cull Off Fog { Mode Off } " +
            	"    BindChannels {" +
            	"      Bind \"vertex\", vertex Bind \"color\", color }" +
            	"} } }" );
        	s_LineMaterial.hideFlags = HideFlags.HideAndDontSave;
        	s_LineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
    	}
	}
	
	// Update is called once per frame
	void OnPostRender() {
		int numLines = (int)(m_GridDimensions / m_GridWidth);
		int halfGridSize = m_GridDimensions/2;
		m_GridColor.a = .8f;
		
		CreateLineMaterial();
		GL.PushMatrix();
		s_LineMaterial.SetPass(0);
		GL.Begin(GL.LINES);
		GL.Color(m_GridColor);
		
		for(int i = -(numLines/2); i <= numLines/2; i++) {
			//-Z lines
			GL.Vertex(new Vector3(i*m_GridWidth, 0, 0));
			GL.Vertex(new Vector3(i*m_GridWidth, 0, -m_GridDimensions));
			
			//+Y lines
			GL.Vertex(new Vector3(i*m_GridWidth, 0, 0));
			GL.Vertex(new Vector3(i*m_GridWidth, m_GridDimensions, 0));
		}
		for(int i = 0; i <= numLines; ++i)
		{
			//X lines on XZ plane
			GL.Vertex(new Vector3(-halfGridSize, 0, -i*m_GridWidth));
			GL.Vertex(new Vector3(halfGridSize, 0, -i*m_GridWidth));
			
			//X lines on XY plane
			GL.Vertex(new Vector3(-halfGridSize, i*m_GridWidth, 0));
			GL.Vertex(new Vector3(halfGridSize, i*m_GridWidth, 0));
		}
		
		GL.End();
		GL.PopMatrix();
	}
	
}
