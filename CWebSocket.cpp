#include <iostream>
#include "CWebSocketServer.hpp"
CWEBSOCKETS_STATIC_DEFINITIONS

using namespace std;

int main() {
    Common::CWebSocketServer m_WSTransceiver;

    m_WSTransceiver.Initialize();
    m_WSTransceiver.StartServer();
    for (int i = 0; i < 1000000000000; i++) {
        m_WSTransceiver.SendData("happiness");
        std::cout << "broadcasting happiness" << std::endl;
    }
    return 0;
}
	
