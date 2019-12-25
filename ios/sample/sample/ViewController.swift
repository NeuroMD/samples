//
//  ViewController.swift
//  sample
//
//  Created by admin on 26/04/2019.
//  Copyright © 2019 admin. All rights reserved.
//

import UIKit
import neurosdk

class ViewController: UIViewController {
    let scanner = NTDeviceEnumerator(deviceType: .TypeBrainbit)
    var device: NTDevice?
    let label: UILabel = {
        let l = UILabel(frame: .init(x: 64, y: 64, width: 100, height: 100))
        return l
        
    }()

    override func viewDidLoad() {
        super.viewDidLoad()
        view.addSubview(label)
        scanner.subscribeFoundDevice { (deviceInfo) in
            self.device = NTDevice(enumerator: self.scanner, deviceInfo)
            guard let device = self.device else {
                return;
            }
            device.connect()
            device.subscribeParameterChanged(subscriber: { (param) in
                if(param == .state) {
                    let state = device.readState
                    if( state == .connected) {
                        print("Connected")
                        for ch in device.channels {
                            print(ch)
                        }
                        for p in device.parameters {
                            print(p)
                        }
                        for c in device.commands {
                            print(c)
                        }
                        DispatchQueue.main.sync {
                            self.label.text = "Connected"
                        }
                        
                    } else {
                        print("Disconnected")
                    }

                }
            })
        }
    }


}

