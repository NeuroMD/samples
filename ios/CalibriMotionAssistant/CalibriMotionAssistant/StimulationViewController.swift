//
//  StimulationViewController.swift
//  CalibriMotionAssistant
//
//  Created by Evgeny Samoylichenko on 13.04.17.
//  Copyright © 2017 Neurotech. All rights reserved.
//

import UIKit

class StimulationViewController: UIViewController {
    
    private var progBtnsArray : [UIButton] = []
    
    private let unselButtonBGColor : UIColor = UIColor.init(red: 200/255, green: 200/255, blue: 200/255, alpha: 0.3)
    
    private let selButtonBGColor : UIColor = UIColor.init(red: 100/255, green: 250/255, blue: 100/255, alpha: 0.7)
    
    private let maxAmpScale : Float = 2.0
    
    
    private let defaults = UserDefaults.standard
    private let progsAmpScalesKeyName = "progsAmpScales"
    private var programsAmpScales : [Float] = [1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0]
    
    override func viewDidLoad() {
        super.viewDidLoad()
//        StimulationProgramsItem.title = NSLocalizedString("rsStimulationProgramsCaption", comment: "")
        progBtnsArray.append(ProgBtn0)
        progBtnsArray.append(ProgBtn1)
        progBtnsArray.append(ProgBtn2)
        progBtnsArray.append(ProgBtn3)
        progBtnsArray.append(ProgBtn4)
        progBtnsArray.append(ProgBtn5)
        progBtnsArray.append(ProgBtn6)
        
        
        if let tmpScales = defaults.array(forKey: progsAmpScalesKeyName) {
            programsAmpScales = tmpScales as! [Float]
        }
        
        StimulationSwither.isEnabled = !maDeviceController.sharedDeviceControllerRef.isInMAMode()
        StimulationSwither.isOn = maDeviceController.sharedDeviceControllerRef.isInStimMode()
        SetNewControlsState(newState: !StimulationSwither.isOn && StimulationSwither.isEnabled)
        
        maDeviceController.sharedDeviceControllerRef.registerStimStateChangedCallback(StimStateChangedCallback: StimStateChangedCallback)
        maDeviceController.sharedDeviceControllerRef.setStimProgUpdateCallback(StimProgUpdateCallback: onUpdateProgress)
        maDeviceController.sharedDeviceControllerRef.setStimStepChangedCallback(stimStepChangedCallback: onStimStepChanged)
        
        
        
        updateProgressViewState(isStimStarted: false)
        slCurAmpl.maximumValue = maxAmpScale
        selectButton(selBtn: progBtnsArray[0])
        // Do any additional setup after loading the view, typically from a nib.
    }
    
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        maDeviceController.sharedDeviceControllerRef.delegateView = self
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    @IBOutlet weak var ProgBtn0: UIButton!
    @IBOutlet weak var ProgBtn1: UIButton!
    @IBOutlet weak var ProgBtn2: UIButton!
    @IBOutlet weak var ProgBtn3: UIButton!
    @IBOutlet weak var ProgBtn4: UIButton!
    @IBOutlet weak var ProgBtn5: UIButton!
    @IBOutlet weak var ProgBtn6: UIButton!
    @IBOutlet weak var lCurAmpl: UILabel!
    
    @IBOutlet weak var StimulationSwither: UISwitch!
    @IBOutlet weak var slCurAmpl: UISlider!
    @IBOutlet weak var lProgramsCaption: UILabel!
    @IBAction func SelectProgram(_ sender: UIButton) {
        selectButton(selBtn: sender)
    }
    @IBOutlet weak var stimProgView: UIProgressView!
    @IBOutlet weak var lStimAction: UILabel!
    @IBOutlet weak var StimulationProgramsItem: UITabBarItem!
    
    @IBAction func onCurAmpScaleChange(_ sender: UISlider) {
        let selProgNum : Int = getProgramNum()
        if (selProgNum >= 0) && (selProgNum < programsAmpScales.count) {
           programsAmpScales[selProgNum] = slCurAmpl.value
            
        }
    }
    
    func StimStateChangedCallback( stimState : Bool ) {
//        switch stimState {
//        case false:
//            NSLog("StimChanged Stim is OFF")
//        case true:
//            NSLog("StimChanged Stim is ON")
//        }
        DispatchQueue.main.async {
            if stimState != self.StimulationSwither.isOn {
                self.StimulationSwither.isOn = stimState
                self.SetNewControlsState(newState: !self.StimulationSwither.isOn)
            }
        }
    }
    
    
    func selectButton(selBtn: UIButton) {
        for theButton in progBtnsArray {
            if theButton == selBtn {
                theButton.backgroundColor = selButtonBGColor
                theButton.tag=1
            }
            else {
                theButton.backgroundColor = unselButtonBGColor
                theButton.tag=0
            }
        }
        let selProgNum : Int = getProgramNum()
        var curAmpScale : Float = 1.0
        if (selProgNum >= 0) && (selProgNum < programsAmpScales.count) {
          curAmpScale = programsAmpScales[selProgNum]
          defaults.set(programsAmpScales, forKey: progsAmpScalesKeyName)
        }
        slCurAmpl.value = curAmpScale
    }
    
    func setNewBtnsStatus(newStatus : Bool) {
        for theButton in progBtnsArray {
            theButton.isEnabled = newStatus
        }
    }
    
    func updateProgressViewState(isStimStarted : Bool) {
        stimProgView.progress = 0
        if !isStimStarted {
            lStimAction.text = NSLocalizedString("rsStimState_STOPPED", comment: "")
        } else {
            lStimAction.text = NSLocalizedString("rsStimState_PREPARING", comment: "") //"Preparing"
        }
        stimProgView.isHidden = !isStimStarted
//        lStimAction.isHidden = !isStimStarted
    }
    
    func SetNewControlsState ( newState : Bool ) {
        setNewBtnsStatus(newStatus: newState)
        lCurAmpl.isEnabled = newState
        slCurAmpl.isEnabled = newState
//        lStimAction.isEnabled = newState
        lProgramsCaption.isEnabled = newState
        updateProgressViewState(isStimStarted: !newState)
    }
    
    func getProgramNum() -> Int {
        for theButton in progBtnsArray {
            if theButton.tag==1 {
                return progBtnsArray.index(of: theButton)!
            }
        }
        return 0
    }
    
    @IBAction func onStimulationSwithChange(_ sender: Any) {
        SetNewControlsState(newState: !StimulationSwither.isOn)
        if StimulationSwither.isOn {
            let stimProgNum = getProgramNum ()
            let AmpScale : Float = slCurAmpl.value
            //            DispatchQueue.global(qos: .userInitiated).async {
            let res = maDeviceController.sharedDeviceControllerRef.startStimProgram(programNum: stimProgNum, curAmpScale: AmpScale)
            if res != true {
                //                    DispatchQueue.main.async {
                self.SetNewControlsState(newState: true)
                self.StimulationSwither.isOn = false
                //                    }
            }
            //            }
        } else {
            maDeviceController.sharedDeviceControllerRef.stopStimProgram()
//            stimProgView.progress = 0
//            lStimAction.text = "Stoped"
        }
    }
    
    func onUpdateProgress(curVal : Int32) {
        DispatchQueue.main.async {
            if self.stimProgView.tag > 0 {
                let curProg : Float = Float(curVal)/Float(self.stimProgView.tag)
                self.stimProgView.progress = curProg
                NSLog("Stim step Changed CurVal = \(curVal) Duration= \(self.stimProgView.tag) Cur step duration = \(curProg)")
            }
        }
    }
    
    func onStimStepChanged( stepDurationInSecs : Int32, isStim : Bool) {
        DispatchQueue.main.async {
            self.stimProgView.progress = 0
            self.stimProgView.tag = Int(stepDurationInSecs)
            NSLog("Stim step Changed Cur step duration = \(stepDurationInSecs)")
            if isStim {
                self.lStimAction.text = NSLocalizedString("rsStimState_STIMULATION", comment: "")  //"Stimulations"
            } else {
                self.lStimAction.text = NSLocalizedString("rsStimState_PAUSE", comment: "")  //"Pause"
            }
        }
    }
}
