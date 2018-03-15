//
//  FirstViewController.swift
//  CalibriMotionAssistant
//
//  Created by Evgeny Samoylichenko on 04.04.17.
//  Copyright © 2017 Neurotech. All rights reserved.
//

import UIKit

class FirstViewController: UIViewController {
    
    var deviceList = [NeuroMotionAssistantDeviceFacade]()
    var tabBarItemConnector: UITabBarItem?
    var tabBarItemPrograms: UITabBarItem?
    
    override func viewDidLoad() {
        super.viewDidLoad()
        maDeviceController.sharedDeviceControllerRef.registerSearchStateChangedCallback(SearchStateChangedCallback: onScanStateChangedCallback)
        maDeviceController.sharedDeviceControllerRef.registerDeviceChangedCallback(devChangedCallback: onDeviceFoundCallback)
        let tabBarControllerItems = self.tabBarController?.tabBar.items
        if let tabArray = tabBarControllerItems {
            self.tabBarItemConnector = tabArray[1]
            self.tabBarItemPrograms = tabArray[2]
            self.tabBarItemConnector?.isEnabled = false
            self.tabBarItemPrograms?.isEnabled = false
        }
    }
    
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        maDeviceController.sharedDeviceControllerRef.delegateView = self
//        self.tableView.reloadData()
    }
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var SearchSwich: UISwitch!
    @IBOutlet weak var DeviceBarItem: UITabBarItem!
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
    }
    
    func onDeviceFoundCallback(){
        DispatchQueue.main.async {
            print("TUT2")
            self.deviceList.removeAll()
            self.deviceList = maDeviceController.sharedDeviceControllerRef.getDeviceList()
            self.tableView.reloadData()
        }
    }
    
    func onScanStateChangedCallback(ScanState : Bool) {
        DispatchQueue.main.async {
            self.SearchSwich.isOn = ScanState
        }
    }
    
    @IBAction func SwithValueChanged(_ sender: Any) {
        if SearchSwich.isOn {
//            maDeviceController.sharedDeviceControllerRef.prepareConnection()
            self.clearDeviceList()
            _ = maDeviceController.sharedDeviceControllerRef.startDeviceSearch()
        } else {
            
            _ = maDeviceController.sharedDeviceControllerRef.stopDeviceSearch()
        }
    }
    
    func clearDeviceList() {
        maDeviceController.sharedDeviceControllerRef.clearDeviceList()
        self.deviceList.removeAll()
        self.tableView.reloadData()
    }
    
    func switchToCorrectorTab(){
        Timer.scheduledTimer(timeInterval: 0.2, target: self, selector: #selector(switchToCorrectorTabCont), userInfo: nil, repeats: false)
    }
    
    func switchToCorrectorTabCont() {
        self.tabBarItemConnector?.isEnabled = true
        self.tabBarItemPrograms?.isEnabled = true
        self.tabBarController!.selectedIndex = 1
    }
}

extension FirstViewController: UITableViewDelegate {
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.deviceList.count
    }
    
    func tableView(_ tableView: UITableView, estimatedHeightForRowAt indexPath: IndexPath) -> CGFloat {
        return UITableViewAutomaticDimension
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return UITableViewAutomaticDimension
    }
}

extension FirstViewController: UITableViewDataSource {
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = self.tableView.dequeueReusableCell(withIdentifier: "deviceName")
        let motionAssistantDevice = self.deviceList[indexPath.row]
        cell?.textLabel?.text = motionAssistantDevice.getDeviceName()
        cell?.detailTextLabel?.text = motionAssistantDevice.getBatteryLevel()
        print("tut \(motionAssistantDevice.getBatteryLevel())")
        return cell!
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        UserDefaults.standard.set(indexPath.row, forKey: "deviceIndex")
        maDeviceController.sharedDeviceControllerRef.changeCurrentDevice(neuroDevice: self.deviceList[indexPath.row])
        self.switchToCorrectorTab()
        
    }

}
