cmake_minimum_required(VERSION 3.3 FATAL_ERROR)

# project name and supported languages
project(device-info C)

# detect operating system and host processor
message(STATUS "We are on a ${CMAKE_SYSTEM_NAME} system")
message(STATUS "The host processor is ${CMAKE_HOST_SYSTEM_PROCESSOR}")

if (WIN32) 
    SET (CMAKE_CONFIGURATION_TYPES "Debug;Release" CACHE STRING
            "Debug;Release" FORCE)
    SET (RELEASE_BUILD_NAME "Release")
    SET(CMAKE_CXX_FLAGS_DEBUG "${CMAKE_CXX_FLAGS_DEBUG} /MDd")
    SET(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} /O2 /MD")
else () 
    SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -fexceptions -fno-rtti")
endif (WIN32)

if (WIN32)
    if(NOT "${CMAKE_GENERATOR}" MATCHES "(Win64|IA64)")
        set (TARGET_PLATFORM "x86")     
        SET (NEUROSDK $ENV{NEUROSDK_86})
    else ()
        set (TARGET_PLATFORM "x64")     
        SET (NEUROSDK $ENV{NEUROSDK_64})
    endif() 
    set (PLATFORM_SUFFIX -${TARGET_PLATFORM})
endif (WIN32)

set(CMAKE_EXE_LINKER_FLAGS "${CMAKE_EXE_LINKER_FLAGS}")
set(CMAKE_CXX_STANDARD_LIBRARIES "${CMAKE_EXE_LINKER_FLAGS}")
set(CMAKE_CONFIGURATION_TYPES "Debug;Release;")

include_directories(${CMAKE_CURRENT_SOURCE_DIR}/include)
if (WIN32)
    include_directories(${NEUROSDK}/include)
    link_directories(${NEUROSDK})
endif (WIN32)

add_executable (device-info main.c)
target_link_libraries(device-info debug neurosdk${PLATFORM_SUFFIX}d)
target_link_libraries(device-info optimized neurosdk${PLATFORM_SUFFIX})