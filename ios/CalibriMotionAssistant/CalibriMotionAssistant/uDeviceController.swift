//
//  uDeviceController.swift
//  CalibriMotionAssistant
//
//  Created by Evgeny Samoylichenko on 07.04.17.
//  Copyright © 2017 Neurotech. All rights reserved.
//

import Foundation
import UIKit

fileprivate let _maDeviceController : maDeviceController = {
    return maDeviceController ()
} ()

struct maParams {
    var pulseDuration_us : NCStimulatorImpulseDuration = NCStimulatorImpulseDuration.us60
    var currentAmp_ma : Int32 = 0
    var pulsFreq_hz : Int32 = 0
    var appSite : Int32 = 0
    var appSite_side : Int32 = 0
    var startAngle : Int32 = 0
    var stopAngle : Int32 = 0
}

class maDeviceController {
    
    public weak var delegateView: UIViewController?
    
    fileprivate var ma_connection: NCMotionAssistantConnection? = nil
    fileprivate var ma_curDevice: NCMotionAssistantDevice? = nil
    
    fileprivate var deviceChangedCallbacks: Array<(Void) -> Void> = []
    fileprivate var serchStateChangedCallbacks: Array<(Bool) -> Void> = []
    fileprivate var stimStateChangedCallbacks: Array<(Bool) -> Void> = []
    
    fileprivate var neuroDeviceStateCallbacks: Array<(NCDeviceState) -> Void> = []
    
    fileprivate var stimProgressUpdate : ((Int32) -> Void)? = nil
    fileprivate var stimStepChanged : ((Int32, Bool) -> Void)? = nil
    
    fileprivate var isScaningDev : Bool = false
    fileprivate var isInStimProgMode : Bool = false
    fileprivate let cScanTimeout : Int32 = 5000
    fileprivate var _minStimulationPause : Int32 = 0;
    fileprivate var _maxStimulationDuration : Int32 = 0;
    fileprivate let cTestStimDuration_ms : Int32 = 2000
    fileprivate var stimulationPrograms : [StimulationProgram] = []
    fileprivate var stimTimer = Timer()
    fileprivate let cStimTimerInterval : TimeInterval = 0.1
    fileprivate var curStimProg : StimulationProgram? = nil
    fileprivate let stimProgramPulseDuration = NCStimulatorImpulseDuration.us200
    fileprivate var startTime : TimeInterval = 0
    fileprivate var stimPhaseDuration : Int32 = 0
    fileprivate var NextStepFlag : Bool = false
    fileprivate var curStepVal : Int32 = 0
    fileprivate var curAmpScaleK : Float = 1
    fileprivate var deviceList = [NeuroMotionAssistantDeviceFacade]()
    
    class var sharedDeviceControllerRef : maDeviceController {
        return _maDeviceController
    }
    
    init () {
        prepareConnection()
        prepareStimPrograms()
    }
    
    func clearDeviceList() {
        if self.deviceList.count > 0 {
            for device in self.deviceList {
                device.neuroMotionAssistantDevice.getNeuroDevice().close()
            }
            self.deviceList.removeAll()
            self.ma_curDevice = nil
        }
    }

    
    public func getDeviceList() -> [NeuroMotionAssistantDeviceFacade] {
        return self.deviceList
    }
    
    func getMotionAssistantDevice(index: Int) -> NeuroMotionAssistantDeviceFacade {
        return self.deviceList[index]
    }
    
    func registerDeviceChangedCallback(devChangedCallback: @escaping (Void)->Void){
        deviceChangedCallbacks.append(devChangedCallback)
    }
    
    func registerSearchStateChangedCallback(SearchStateChangedCallback: @escaping (Bool)->Void){
        serchStateChangedCallbacks
            .append(SearchStateChangedCallback)
    }
    
    func registerStimStateChangedCallback(StimStateChangedCallback: @escaping (Bool)->Void){
        stimStateChangedCallbacks
            .append(StimStateChangedCallback)
    }
    
    func setStimProgUpdateCallback(StimProgUpdateCallback : @escaping (Int32)->Void) {
        stimProgressUpdate = StimProgUpdateCallback
    }
    
    func setStimStepChangedCallback(stimStepChangedCallback : @escaping (Int32, Bool)->Void) {
        stimStepChanged = stimStepChangedCallback
    }
    
    func getCurrentMotionAssistantDevice() -> NCMotionAssistantDevice? {
        return self.ma_curDevice
    }
    
    func changeCurrentDevice(neuroDevice: NeuroMotionAssistantDeviceFacade) {
        self.ma_curDevice = neuroDevice.neuroMotionAssistantDevice
    }
    
    func onDeviceFound(foundNeuroDevice: NCMotionAssistantDevice?) {
        if let foundDevice = foundNeuroDevice {
            let neuroDevice = NeuroMotionAssistantDeviceFacade(neuroMotionAssistantDevice: foundDevice)
            neuroDevice.neuroMotionAssistantDevice.getNeuroDevice().subscribeStateChanged({ state in
                neuroDevice.handleNeuroDeviceState(state: state)
            })
            self.deviceList.append(neuroDevice)
        }
        PrepareDevice()
        notifyAll_DeviceChanged()
    }
    
    
    func notifyAll_searchStateChanged(){
        if (serchStateChangedCallbacks.isEmpty) {return}
        for subscriber in serchStateChangedCallbacks {subscriber(isScaningDev)}
    }
    
    func notifyAll_DeviceChanged(){
        if (deviceChangedCallbacks.isEmpty) {return}
        for subscriber in deviceChangedCallbacks {subscriber()}
    }
    
    func onScanStateChangedCallback(ScanState : Bool) {
        isScaningDev = ScanState
        notifyAll_searchStateChanged()
    }
    
    func disconnect() {
        ma_connection = nil
    }
    
    func prepareConnection() {
        if ma_connection == nil {
            ma_connection = NCMotionAssistantConnection()
            ma_connection?.setDeviceFoundCallback(onDeviceFound)
            ma_connection?.setScanStateChangedCallback(onScanStateChangedCallback)
        }
    }
    
    func onStimulatorStateChangedCallBack(StimState : Bool) {
        if StimState {
            NSLog("On stimulation changed callback StimState=True")
        }
        else {
            NSLog("On stimulation changed callback StimState=False")
        }
        notifyAll_StimStateChanged(stimState: StimState)
    }
    
    func notifyAll_StimStateChanged(stimState : Bool){
        if (stimStateChangedCallbacks.isEmpty) {return}
        for subscriber in stimStateChangedCallbacks {subscriber(stimState)}
    }
    
    func PrepareDevice() {
        if ma_curDevice != nil {
            ma_curDevice?.setStimulatorStateChangedCallback(onStimulatorStateChangedCallBack)
        }
    }
    
    func startDeviceSearch() -> Bool {
        var res = ma_connection != nil
        if !res {
            return res
        }
        
        if !isScaningDev {
            ma_curDevice = nil
            ma_connection?.startScan(cScanTimeout)
            res = true
        }
        else {
            ma_connection?.stopScan()
            res = false
        }
        return res
    }
    
    func stopDeviceSearch() -> Bool {
        if ma_connection == nil {
            return false
        }
        
        if isScaningDev {
            ma_connection?.stopScan()
        }
        return true
    }
    
    func getDeviceCurParams(index: Int) -> maParams? {
        let neuroDevice: NCMotionAssistantDevice = self.deviceList[index].neuroMotionAssistantDevice
        var curParams : maParams? = nil
        ma_curDevice = neuroDevice
        if ma_curDevice != nil {
            curParams = maParams()
            curParams?.currentAmp_ma = (ma_curDevice?.getCurrentAmplitude())!
            curParams?.pulseDuration_us = (ma_curDevice?.getPulseDuration())!
            curParams?.pulsFreq_hz = (ma_curDevice?.getPulseFrequency())!
            curParams?.startAngle = (ma_curDevice?.getGyroStartThreshold())!
            curParams?.stopAngle = (ma_curDevice?.getGyroStopThreshold())!
            
            _minStimulationPause = (ma_curDevice?.getMinAssistantStimulationPause())!
            _maxStimulationDuration = (ma_curDevice?.getMaxAssistantStimulusDuration())!
            
            let maLimb : NCMotionAssistantLimb = (ma_curDevice?.getLimbForStimulation())!
            let maLimbIdx = maLimb.rawValue
            curParams?.appSite = Int32(maLimbIdx/2);
            curParams?.appSite_side = Int32(maLimbIdx % 2);
        }
        return curParams
    }
    
    func setDeviceCurParams ( newParams : maParams ) {
        if ma_curDevice != nil {
            let maLimbIdx = newParams.appSite_side + newParams.appSite*2
            let maLimb = NCMotionAssistantLimb(rawValue : UInt(maLimbIdx))
            
            ma_curDevice?.setMotionAssistantParams(newParams.startAngle, gyroStop: newParams.stopAngle, limb: maLimb!, minStimulationPause: _minStimulationPause, maxStimulationDuration: _maxStimulationDuration)
            ma_curDevice?.setCurrentAmplitude(newParams.currentAmp_ma)
            ma_curDevice?.setPulseFrequency(newParams.pulsFreq_hz)
            ma_curDevice?.setPulseDuration(newParams.pulseDuration_us)
            
            
        }
    }
    
    func getStimTestDur_ms() -> Int32 {
        return cTestStimDuration_ms
    }
    
    func isInStimMode () -> Bool {
        if ma_curDevice != nil {
            return (ma_curDevice?.getStimulatorState())!
        }
        return false
    }
    
    func StopStim() {
        let inStimMode : Bool = isInStimMode()
        if inStimMode == true {
            ma_curDevice?.stimulationStop()
        }
    }
    
    func TestStimParams() -> Bool {
        NSLog("Test stim params")
        let inStimMode : Bool = isInStimMode()
        if inStimMode == true {
            NSLog("Stimulation STOP")
            ma_curDevice?.stimulationStop()
            return false
        }
        else {
            ma_curDevice?.setStimulationDuration(cTestStimDuration_ms)
            NSLog("Stimulation START")
            ma_curDevice?.stimulationStart()
            return true
        }
    }
    
    func isInMAMode() -> Bool {
        if ma_curDevice != nil {
            return (ma_curDevice?.getMotionAssistantState())!
        }
        return false
    }
    func SetMAState(newMAState : Bool) -> Bool {
        if ma_curDevice == nil {return false}
        if newMAState {
            ma_curDevice?.motionAssistantStart()
        }
        else {
            ma_curDevice?.motionAssistantStop()
        }
        return isInMAMode()
    }
    
    func prepareStimPrograms () {
        var relaxProgram = StimulationProgram()
        relaxProgram.addStimul(stimul: Stimul(frequency: 3, ampl: 15, duration: 30, pause: 30))
        relaxProgram.addStimul(stimul: Stimul(frequency: 3, ampl: 15, duration: 30, pause: 30))
        relaxProgram.addStimul(stimul: Stimul(frequency: 3, ampl: 15, duration: 30, pause: 30))
        relaxProgram.addStimul(stimul: Stimul(frequency: 3, ampl: 15, duration: 30, pause: 30))
        relaxProgram.addStimul(stimul: Stimul(frequency: 3, ampl: 15, duration: 30, pause: 30))
        stimulationPrograms.append(relaxProgram)
        
        var synapsProgram = StimulationProgram()
        synapsProgram.addStimul(stimul: Stimul(frequency: 3, ampl: 15, duration: 30, pause: 20))
        synapsProgram.addStimul(stimul: Stimul(frequency: 5, ampl: 15, duration: 20, pause: 20))
        synapsProgram.addStimul(stimul: Stimul(frequency: 10, ampl: 15, duration: 15, pause: 20))
        synapsProgram.addStimul(stimul: Stimul(frequency: 20, ampl: 15, duration: 10, pause: 60))
        stimulationPrograms.append(synapsProgram)
        
        var dynamicProgram = StimulationProgram()
        dynamicProgram.addStimul(stimul: Stimul(frequency: 3, ampl: 10, duration: 15, pause: 15))
        dynamicProgram.addStimul(stimul: Stimul(frequency: 3, ampl: 15, duration: 15, pause: 15))
        dynamicProgram.addStimul(stimul: Stimul(frequency: 5, ampl: 15, duration: 10, pause: 15))
        dynamicProgram.addStimul(stimul: Stimul(frequency: 15, ampl: 10, duration: 15, pause: 15))
        dynamicProgram.addStimul(stimul: Stimul(frequency: 3, ampl: 15, duration: 15, pause: 15))
        dynamicProgram.addStimul(stimul: Stimul(frequency: 6, ampl: 15, duration: 10, pause: 30))
        stimulationPrograms.append(dynamicProgram)
        
        var highFreqProgram = StimulationProgram()
        highFreqProgram.addStimul(stimul: Stimul(frequency: 15, ampl: 10, duration: 15, pause: 5))
        highFreqProgram.addStimul(stimul: Stimul(frequency: 15, ampl: 15, duration: 15, pause: 10))
        highFreqProgram.addStimul(stimul: Stimul(frequency: 20, ampl: 10, duration: 10, pause: 10))
        stimulationPrograms.append(highFreqProgram)
        
        var tonicProgram = StimulationProgram()
        tonicProgram.addStimul(stimul: Stimul(frequency: 3, ampl: 10, duration: 15, pause: 15))
        tonicProgram.addStimul(stimul: Stimul(frequency: 6, ampl: 15, duration: 15, pause: 15))
        tonicProgram.addStimul(stimul: Stimul(frequency: 10, ampl: 15, duration: 10, pause: 15))
        tonicProgram.addStimul(stimul: Stimul(frequency: 15, ampl: 10, duration: 10, pause: 15))
        tonicProgram.addStimul(stimul: Stimul(frequency: 20, ampl: 10, duration: 10, pause: 30))
        stimulationPrograms.append(tonicProgram)
        
        var deepProgram = StimulationProgram()
        deepProgram.addStimul(stimul: Stimul(frequency: 3, ampl: 30, duration: 5, pause: 10))
        deepProgram.addStimul(stimul: Stimul(frequency: 3, ampl: 35, duration: 5, pause: 10))
        deepProgram.addStimul(stimul: Stimul(frequency: 3, ampl: 40, duration: 5, pause: 15))
        deepProgram.addStimul(stimul: Stimul(frequency: 3, ampl: 35, duration: 5, pause: 10))
        deepProgram.addStimul(stimul: Stimul(frequency: 3, ampl: 30, duration: 5, pause: 60))
        stimulationPrograms.append(deepProgram)
        
        var analgesiaProgram = StimulationProgram()
        analgesiaProgram.addStimul(stimul: Stimul(frequency: 100, ampl: 5, duration: 5, pause: 2))
        analgesiaProgram.addStimul(stimul: Stimul(frequency: 150, ampl: 5, duration: 7, pause: 2))
        analgesiaProgram.addStimul(stimul: Stimul(frequency: 150, ampl: 5, duration: 8, pause: 2))
        analgesiaProgram.addStimul(stimul: Stimul(frequency: 100, ampl: 5, duration: 10, pause: 2))
        analgesiaProgram.addStimul(stimul: Stimul(frequency: 150, ampl: 5, duration: 7, pause: 2))
        analgesiaProgram.addStimul(stimul: Stimul(frequency: 150, ampl: 5, duration: 8, pause: 2))
        analgesiaProgram.addStimul(stimul: Stimul(frequency: 150, ampl: 5, duration: 5, pause: 2))
        analgesiaProgram.addStimul(stimul: Stimul(frequency: 150, ampl: 5, duration: 10, pause: 2))
        stimulationPrograms.append(analgesiaProgram)
    }
    
    func startStimProgram(programNum : Int, curAmpScale : Float) -> Bool {
        let res : Bool = (programNum >= 0 && programNum < stimulationPrograms.count)
        if res {
            curStimProg = stimulationPrograms[programNum]
            isInStimProgMode = true
            curAmpScaleK = curAmpScale
            curStimProg?.reset()
            startTime = 0
            stimTimer = Timer.scheduledTimer(timeInterval: cStimTimerInterval, target: self, selector: #selector(self.stimProgramExecutor), userInfo: NSDate(), repeats: true)
            return true
        }
        return false
    }
    
    func stopStimProgram() {
        stimTimer.invalidate()
        isInStimProgMode = false
        ma_curDevice?.stimulationStop()
    }
    
    func getTimePassInterval() -> TimeInterval {
        var timePass : TimeInterval = 0
        if stimTimer.isValid {
            let curDate : NSDate = stimTimer.userInfo as! NSDate
            timePass = -curDate.timeIntervalSinceNow
        }
        return timePass
    }
    
    @objc func stimProgramExecutor () {
        if curStimProg == nil {
            stimTimer.invalidate()
        } else {
            let timePass : TimeInterval = getTimePassInterval()
            var prepareNewStimul : Bool = false
            let timePassMS : Int32 = Int32((timePass - startTime))
            var curStepProgVal : Int32 = timePassMS
            
            if startTime == 0 {
                prepareNewStimul = true
            } else {
                
                let curStepDurMS : Int32 = curStimProg!.getCurDuration_ms()
                prepareNewStimul = timePassMS >= curStepDurMS
                
                if (timePassMS - stimPhaseDuration) >= 0 {
                    if NextStepFlag {
//                        ma_curDevice?.stimulationStop()
                        NextStepFlag = false
                        onStimStepChanged(stepDurationInSecs: curStepDurMS - stimPhaseDuration, isStim: false)
                    }
                    curStepProgVal = timePassMS - stimPhaseDuration
                }
            }
            
            if curStepVal < timePassMS {
                curStepVal = timePassMS
                onUpdateProgress(curVal: curStepProgVal)
            }
            
            if prepareNewStimul {
                let curStim : Stimul? = curStimProg?.next()
                if curStim == nil {
                    stopStimProgram()
                    return
                } else {
                    startTime = getTimePassInterval()
                    applyNewStimul(curStim: curStim!)
                    stimPhaseDuration = curStim!.Duration
                    onStimStepChanged(stepDurationInSecs: stimPhaseDuration, isStim: true)
                    NextStepFlag = true
                    curStepVal = 0
                }
                
            }
        }
    }
    func applyNewStimul(curStim : Stimul) {
        ma_curDevice?.setCurrentAmplitude(Int32(Float(curStim.Amplitude)*curAmpScaleK))
        ma_curDevice?.setPulseFrequency(curStim.Frequency)
        ma_curDevice?.setPulseDuration(stimProgramPulseDuration)
        ma_curDevice?.setStimulationDuration(curStim.Duration * 1000)
        ma_curDevice?.stimulationStart()
        
    }
    
    func onUpdateProgress(curVal : Int32) {
        if stimProgressUpdate != nil {
            DispatchQueue.global(qos: .userInitiated).async {
                self.stimProgressUpdate!(curVal)
            }
        }
    }
    func onStimStepChanged( stepDurationInSecs : Int32, isStim : Bool) {
        if stimStepChanged != nil {
            DispatchQueue.global(qos: .userInitiated).async {
                self.stimStepChanged!(stepDurationInSecs, isStim)
            }
        }
    }
    
    func removeNeuroDeviceFacade(neuroMotionAssistantDeviceFacade: NeuroMotionAssistantDeviceFacade) {
        for i in 0..<self.deviceList.count {
            if self.deviceList[i] == neuroMotionAssistantDeviceFacade {
                self.deviceList.remove(at: i)
                print("TUT")
                self.notifyAll_DeviceChanged()
            }
        }
    }
    
    func openFirstViewController() {
        DispatchQueue.main.async {
            if let delegate = self.delegateView {
//                print("Current state - \(state.rawValue)")
                let tabBarControllerItems = delegate.tabBarController?.tabBar.items
                if let tabArray = tabBarControllerItems {
                    let tabBar1 = tabArray[1]
                    let tabBar2 = tabArray[2]
                    tabBar1.isEnabled = false
                    tabBar2.isEnabled = false
                }
                delegate.tabBarController?.selectedIndex = 0
            }
        }

    }
}

class NeuroMotionAssistantDeviceFacade {
    let neuroMotionAssistantDevice: NCMotionAssistantDevice
//    let neuroDevice: NCNeuroDevice
    
    init(neuroMotionAssistantDevice: NCMotionAssistantDevice) {
        self.neuroMotionAssistantDevice = neuroMotionAssistantDevice
    }
    
    public func getDeviceName() -> String {
        let deviceNameArray = self.neuroMotionAssistantDevice.getNeuroDevice().getName().components(separatedBy: "_")
        let lastElement = deviceNameArray.last
        var color: String = "Blue"
        if let last = lastElement {
            switch last.lowercased() {
            case "w":
                color = "White"
                break
            case "y":
                color = "Yellow"
                break
            case "r":
                color = "Red"
                break
            default:
                color = "Blue"
                break
            }
        }
        return "Neurotech Callibri \(color)"
    }
    
    public func getBatteryLevel() -> String {
        return "\(self.neuroMotionAssistantDevice.getNeuroDevice().getBatteryLevel())%"
    }
    
    
    func handleNeuroDeviceState(state: NCDeviceState) {
        print("Current state - \(state.rawValue)")
        if state != .Ready {
            maDeviceController.sharedDeviceControllerRef.removeNeuroDeviceFacade(neuroMotionAssistantDeviceFacade: self)
            if let currentNeuroDevice: NCMotionAssistantDevice = maDeviceController.sharedDeviceControllerRef.getCurrentMotionAssistantDevice() {
                if currentNeuroDevice == self.neuroMotionAssistantDevice {
                    maDeviceController.sharedDeviceControllerRef.openFirstViewController()
                }
            }
        } else {
            maDeviceController.sharedDeviceControllerRef.notifyAll_DeviceChanged()
        }
    }
}

extension NeuroMotionAssistantDeviceFacade: Equatable {}
func ==(lhs: NeuroMotionAssistantDeviceFacade, rhs: NeuroMotionAssistantDeviceFacade) -> Bool {
    return lhs.neuroMotionAssistantDevice.getNeuroDevice().getAddress() == rhs.neuroMotionAssistantDevice.getNeuroDevice().getAddress()
}
