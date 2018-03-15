//
//  SecondViewController.swift
//  CalibriMotionAssistant
//
//  Created by Evgeny Samoylichenko on 04.04.17.
//  Copyright © 2017 Neurotech. All rights reserved.
//

import UIKit

class SecondViewController: UIViewController {
    
    override func viewDidLoad() {
        super.viewDidLoad()
        MotionAssistantSwith.isOn = maDeviceController.sharedDeviceControllerRef.isInMAMode()
        setControlsState(newControlState: !MotionAssistantSwith.isOn)
        maDeviceController.sharedDeviceControllerRef.registerStimStateChangedCallback(StimStateChangedCallback: StimStateChangedCallback)
        maDeviceController.sharedDeviceControllerRef.registerDeviceChangedCallback(devChangedCallback: onDeviceFoundCallback)
    }
    
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        maDeviceController.sharedDeviceControllerRef.delegateView = self
        let index = UserDefaults.standard.integer(forKey: "deviceIndex")
        let curParams = maDeviceController.sharedDeviceControllerRef.getDeviceCurParams(index: index)
        WriteParamsToUI(theParams: curParams!)
    }
    
    @IBOutlet weak var lCurrentAmpVal: UILabel!
    @IBOutlet weak var slCurrentAmpVal: UISlider!
    @IBOutlet weak var lFreqVal: UILabel!
    @IBOutlet weak var slFreqVal: UISlider!
    @IBOutlet weak var lStartAngleVal: UILabel!
    @IBOutlet weak var slStartAngleVal: UISlider!
    @IBOutlet weak var lStopAngleVal: UILabel!
    @IBOutlet weak var slStopAngleVal: UISlider!
    @IBOutlet weak var pulseDurationValue: UISegmentedControl!
    @IBOutlet weak var AppSiteVal: UISegmentedControl!
    @IBOutlet weak var AppSiteSide: UISegmentedControl!
//    @IBOutlet weak var ReadDeviceParams: UIButton!
    @IBOutlet weak var WriteDeviceParams: UIButton!
    @IBOutlet weak var TestParams: UIButton!
    @IBOutlet weak var MotionAssistantSwith: UISwitch!
    @IBOutlet weak var MotionAssistantItem: UITabBarItem!
    
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
    }
    
    @IBAction func onCurAmpValChange(_ sender: Any) {
        lCurrentAmpVal.text = (round(slCurrentAmpVal.value)  as NSNumber).stringValue + " " + NSLocalizedString("rsCurrent_Dim_mA", comment: "") // mA
    }
    
    @IBAction func onFreqValChange(_ sender: Any) {
        lFreqVal.text = (round(slFreqVal.value) as NSNumber).stringValue + " " + NSLocalizedString("rsFreq_Dim_Hz", comment: "") //" Hz"
    }
    
    @IBAction func onStartAngleValChange(_ sender: Any) {
        lStartAngleVal.text = (round(slStartAngleVal.value) as NSNumber).stringValue
    }
    
    @IBAction func onStopAngleValChange(_ sender: Any) {
        lStopAngleVal.text = (round(slStopAngleVal.value) as NSNumber).stringValue
    }
    
    func onDeviceFoundCallback(){
        //        DispatchQueue.main.async {
        //            self.setControlsState(newControlState: maDeviceController.sharedDeviceControllerRef.)
        //        }
    }
    
    func setControlsState( newControlState : Bool ) {
        lCurrentAmpVal.isEnabled = newControlState
        slCurrentAmpVal.isEnabled = newControlState
        
        lFreqVal.isEnabled = newControlState
        slFreqVal.isEnabled = newControlState
        
        lStartAngleVal.isEnabled = newControlState
        slStartAngleVal.isEnabled = newControlState
        
        lStopAngleVal.isEnabled = newControlState
        slStopAngleVal.isEnabled = newControlState
        
        pulseDurationValue.isEnabled = newControlState
        AppSiteVal.isEnabled = newControlState
        AppSiteSide.isEnabled = newControlState
        
//        ReadDeviceParams.isEnabled = newControlState
        WriteDeviceParams.isEnabled = newControlState
        TestParams.isEnabled = newControlState
    }
    
    func ReadParamsFromUI () -> maParams? {
        var theParams = maParams()
        
        var curPulsDur : NCStimulatorImpulseDuration
        switch pulseDurationValue.selectedSegmentIndex {
        case 0:
            curPulsDur = .us60
        case 1:
            curPulsDur = .us100
        case 2:
            curPulsDur = .us200
        default:
            curPulsDur = .us60
        }
        theParams.pulseDuration_us = curPulsDur
        
        theParams.currentAmp_ma = Int32(round(slCurrentAmpVal.value))
        theParams.pulsFreq_hz = Int32(round(slFreqVal.value))
        theParams.appSite = Int32(AppSiteVal.selectedSegmentIndex)
        theParams.appSite_side = Int32(1 - AppSiteSide.selectedSegmentIndex)
        theParams.startAngle = Int32(round(slStartAngleVal.value))
        theParams.stopAngle = Int32(round(slStopAngleVal.value))
        return theParams
    }
    
    func WriteParamsToUI ( theParams : maParams) {
        
        switch theParams.pulseDuration_us {
        case .us60:
            pulseDurationValue.selectedSegmentIndex = 0
        case .us100:
            pulseDurationValue.selectedSegmentIndex = 1
        case .us200:
            pulseDurationValue.selectedSegmentIndex = 2
        }
        
        slCurrentAmpVal.value = Float(theParams.currentAmp_ma)
        slFreqVal.value = Float(theParams.pulsFreq_hz)
        slStartAngleVal.value = Float(theParams.startAngle)
        slStopAngleVal.value = Float(theParams.stopAngle)
        AppSiteVal.selectedSegmentIndex = Int(theParams.appSite)
        AppSiteSide.selectedSegmentIndex = 1 - Int(theParams.appSite_side)
        
        onCurAmpValChange(slCurrentAmpVal)
        onFreqValChange(slFreqVal)
        onStartAngleValChange(slStartAngleVal)
        onStopAngleValChange(slStopAngleVal)
    }
    
//    @IBAction func onReadDeviceParams(_ sender: Any) {
//        let theDevParams = maDeviceController.sharedDeviceControllerRef.getDeviceCurParams()
//        if theDevParams != nil {
//            WriteParamsToUI(theParams: theDevParams!)
//        }
//    }
    
    @IBAction func onWriteDeviceParams(_ sender: Any) {
        let theDevParams = ReadParamsFromUI()
        if theDevParams != nil{
            maDeviceController.sharedDeviceControllerRef.setDeviceCurParams(newParams: theDevParams!)
        }
    }
    
    @IBAction func onTestDeviceParams(_ sender: Any) {
        let isStimMode = maDeviceController.sharedDeviceControllerRef.TestStimParams()
        if isStimMode {
            setControlsState(newControlState: false)
            MotionAssistantSwith.isEnabled = false
            let stimDurIn_ms : Int = Int(maDeviceController.sharedDeviceControllerRef.getStimTestDur_ms())
            DispatchQueue.main.asyncAfter(deadline: .now() + .milliseconds(stimDurIn_ms), execute: {
                maDeviceController.sharedDeviceControllerRef.StopStim()
                self.setControlsState(newControlState: true)
                self.MotionAssistantSwith.isEnabled = true
            })
        }
    }
    
    func StimStateChangedCallback( stimState : Bool ) {
        DispatchQueue.main.async {
            self.setControlsState(newControlState: !stimState)
            self.MotionAssistantSwith.isEnabled = !stimState
        }
    }
    
    @IBAction func onMoionAssistantSwithChanged(_ sender: Any) {
        let maState = maDeviceController.sharedDeviceControllerRef.SetMAState(newMAState: MotionAssistantSwith.isOn)
        setControlsState(newControlState: !maState)
        MotionAssistantSwith.isOn = maState
    }
}

