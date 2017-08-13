On the client side one has to send the global joint positions:

EXAMPLE:

//global variables and include mongoose library (the library available in the root of the folder)
#include "CWebSocketServer.hpp"

Common::CWebSocketServer m_WSTransceiver;
	***************************************
	//somewhere in the code updating/sending the joint positions for the WoodenMan
	***************************************
	std::stringstream wss;
	auto& Joints = m_skeleton->getJoints(); //getting the joints
	for (int ijk = 0; ijk < Joints.size(); ++ijk)
	{
		auto Pos3D = Joints[ijk]->getGlobalPosition();
		wss << Pos3D[0] << ", " << Pos3D[1] << ", " << Pos3D[2];
		if (ijk != (Joints.size() - 1))
			wss << ", ";
	}
	m_WSTransceiver.SendData(wss.str());
	****************************************
	
	
	The order of joints defined in the WoodenMan\WoodenMan\Assets\Scripts\runLiveVNect.cs 
	
	After building the main project WoodenMan, one can run the WoodenMan.exe.
	Before sending anything to the WoodenMan, one has to start WoodenMan.exe beforehand.
	Only after that one shall start client.exe (your program) which will send the joint positions.
