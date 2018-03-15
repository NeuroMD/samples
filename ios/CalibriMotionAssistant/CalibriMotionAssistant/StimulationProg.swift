//
//  StimulationProg.swift
//  CalibriMotionAssistant
//
//  Created by Evgeny Samoylichenko on 14.04.17.
//  Copyright © 2017 Neurotech. All rights reserved.
//

import Foundation

struct StimulationProgram{
    
    private var stimulus: Array<Stimul>
    private var currentStimul: Int
    private var timeToEndCurStep: Int32
    
    init(){
        stimulus = Array<Stimul>()
        currentStimul = 0
        timeToEndCurStep = 0
    }
    
    mutating func addStimul(stimul: Stimul){
        stimulus.append(stimul)
        currentStimul = 0
    }
    
    mutating func reset(){
        currentStimul = 0
        timeToEndCurStep = 0
    }
    
    mutating func next()->Stimul?{
        if (currentStimul >= stimulus.count)
        {
            return nil
        }
        
        let stimul = stimulus[currentStimul]
        currentStimul+=1
        timeToEndCurStep = stimul.Duration + stimul.Pause
        return stimul
    }
    mutating func getCurDuration_ms () -> Int32 {
        return timeToEndCurStep
    }
}

struct Stimul{
    var Frequency: Int32
    var Amplitude: Int32
    var Duration: Int32
    var Pause: Int32
    
    init(frequency: Int32, ampl: Int32, duration: Int32, pause: Int32){
        Frequency = frequency
        Amplitude = ampl
        Duration = duration
        Pause = pause
    }
}
