cmake_minimum_required(VERSION 3.3 FATAL_ERROR)

if (WIN32)
    if(NOT "${CMAKE_GENERATOR}" MATCHES "(Win64|IA64)")
		set (TARGET_PLATFORM "x86")		
		SET (NEUROSDK $ENV{NEUROSDK_86})
	else ()
		set (TARGET_PLATFORM "x64")		
		SET (NEUROSDK $ENV{NEUROSDK_64})
	endif()	
endif (WIN32)

# project name and supported languages
project(resistance-${TARGET_PLATFORM} CXX)

# detect operating system and host processor
message(STATUS "We are on a ${CMAKE_SYSTEM_NAME} system")
message(STATUS "The host processor is ${CMAKE_HOST_SYSTEM_PROCESSOR}")

if (WIN32) 
    SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /O2 /MD")
else () 
	SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=gnu++14 -Wall")
	SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -fexceptions -fno-rtti")
endif (WIN32)

set(CMAKE_CONFIGURATION_TYPES "Debug;Release;")

include_directories(${CMAKE_CURRENT_SOURCE_DIR}/include)
if (WIN32)
    include_directories(${NEUROSDK}/include)
    link_directories(${NEUROSDK})
endif (WIN32)

add_executable (resistance main.cpp)
target_link_libraries(resistance debug neurosdkd optimized neurosdk)

add_custom_command(TARGET resistance POST_BUILD        
    COMMAND ${CMAKE_COMMAND} -E copy_if_different  
        "${NEUROSDK}/neurosdkd.dll"      
        $<TARGET_FILE_DIR:resistance>)
add_custom_command(TARGET resistance POST_BUILD        
    COMMAND ${CMAKE_COMMAND} -E copy_if_different  
        "${NEUROSDK}/neurosdk.dll"      
        $<TARGET_FILE_DIR:resistance>)