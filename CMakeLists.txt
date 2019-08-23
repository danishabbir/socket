cmake_minimum_required(VERSION 3.14)

set(CMAKE_CXX_STANDARD 11)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
message(STATUS "C++11 support has been enabled by default.")

project(websocket)

add_subdirectory(mongoose)
add_executable(socket CWebSocket.cpp)
target_link_libraries(socket mongoose)

### COLLECT ALL 3RD-PARTY LIBRARIES TO BE LINKED AGAINST
SET(Mongoose_INC_PATH ${CMAKE_SOURCE_DIR}/mongoose)
SET(Mongoose_LIB_PATH ${CMAKE_SOURCE_DIR}/mongoose/lib)
LINK_DIRECTORIES(${Mongoose_LIB_PATH})
SET(Mongoose_LIBS ${CMAKE_DL_LIBS} ${CMAKE_THREAD_LIBS_INIT} mongoose)
#MESSAGE(STATUS "Found Mongoose at Mongoose_INC_PATH=${Mongoose_INC_PATH} and libs at Mongoose_LIB_PATH=${Mongoose_LIB_PATH}. Don't forget to add libraries to your build.")
MESSAGE(STATUS "Found Mongoose.")
INCLUDE_DIRECTORIES(${Mongoose_INC_PATH})
set(websocket_3rdparty_libraries ${Mongoose_LIBS})

install(TARGETS socket DESTINATION bin)
