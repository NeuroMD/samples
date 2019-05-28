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
    let scanner = NTDeviceScanner()
    var device: NTDevice?
    let label: UILabel = {
        let l = UILabel(frame: .init(x: 64, y: 64, width: 100, height: 100))
        return l
        
    }()
    override func viewDidLoad() {
        super.viewDidLoad()
        
        view.addSubview(label)
        scanner.subscribeFoundDevice { [weak self] (device) in
            if let safe = self {
                safe.device = device
                guard let d = safe.device else {return}
                
                safe.device?.connect()
                safe.device?.subscribeParameterChanged(subscriber: { (param) in
                    if(param == .State) {
                        let state = safe.device?.readParam(param: .State) as NTState?
                        if( state == .Connected) {
                            
                            for ch in d.channels() {
                                print(ch)
                            }
                            for p in d.parameters() {
                                print(p)
                            }
                            for c in d.commands() {
                                print(c)
                            }
                            
                            let name = safe.device?.readParam(param: .Name) as String?
                            if(Thread.isMainThread) {
                                safe.label.text = name
                                
                            } else {
                                DispatchQueue.main.sync {
                                    safe.label.text = name
                                }
                            }
                        }
                        
                    }
                })
            }
        }
    }


}

