cmake_minimum_required(VERSION 3.3 FATAL_ERROR)

# project name and supported languages
project(device-info C)

# detect operating system and host processor
message(STATUS "We are on a ${CMAKE_SYSTEM_NAME} system")
message(STATUS "The host processor is ${CMAKE_HOST_SYSTEM_PROCESSOR}")

if (WIN32) 
    SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /O2 /MD")
else () 
	SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c99 -Wall")
endif (WIN32)

if (WIN32)
    if(NOT "${CMAKE_GENERATOR}" MATCHES "(Win64|IA64)")
        set (TARGET_PLATFORM "x86")     
        SET (NEUROSDK $ENV{NEUROSDK_86})
    else ()
        set (TARGET_PLATFORM "x64")     
        SET (NEUROSDK $ENV{NEUROSDK_64})
    endif() 
endif (WIN32)

set(CMAKE_EXE_LINKER_FLAGS "${CMAKE_EXE_LINKER_FLAGS}")
set(CMAKE_CXX_STANDARD_LIBRARIES "${CMAKE_EXE_LINKER_FLAGS}")
set(CMAKE_CONFIGURATION_TYPES "Debug;Release;")

include_directories(${CMAKE_CURRENT_SOURCE_DIR}/include)
if (WIN32)
    include_directories(${NEUROSDK}/include/wrappers/c)
    link_directories(${NEUROSDK})
endif (WIN32)

add_executable (device-info main.c)
target_link_libraries(device-info debug c-neurosdkd)
target_link_libraries(device-info optimized c-neurosdk)

add_custom_command(TARGET device-info POST_BUILD        
    COMMAND ${CMAKE_COMMAND} -E copy_if_different  
        "${NEUROSDK}/c-neurosdkd.dll"      
        $<TARGET_FILE_DIR:device-info>)
add_custom_command(TARGET device-info POST_BUILD        
    COMMAND ${CMAKE_COMMAND} -E copy_if_different  
        "${NEUROSDK}/c-neurosdk.dll"      
        $<TARGET_FILE_DIR:device-info>)
add_custom_command(TARGET device-info POST_BUILD        
    COMMAND ${CMAKE_COMMAND} -E copy_if_different  
        "${NEUROSDK}/neurosdkd.dll"      
        $<TARGET_FILE_DIR:device-info>)
add_custom_command(TARGET device-info POST_BUILD        
    COMMAND ${CMAKE_COMMAND} -E copy_if_different  
        "${NEUROSDK}/neurosdk.dll"      
        $<TARGET_FILE_DIR:device-info>)