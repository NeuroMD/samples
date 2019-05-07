//
//  RootViewController.swift
//  sample1
//
//  Created by admin on 22/04/2019.
//  Copyright © 2019 admin. All rights reserved.
//

import UIKit
import CoreBluetooth
import Metal
import neurosdk


class RootViewController: UIViewController{
    //MARK: for render
    var metalDevice: MTLDevice!
    var metalLayer: CAMetalLayer!
    var vertexBuffer: MTLBuffer!
    var pipelineState: MTLRenderPipelineState!
    var commandQueue: MTLCommandQueue!
    var timer: CADisplayLink!
    
    //MARK: neurosdk classes
    var scanner: NTDeviceScanner? = nil
    var device: NTDevice?
    var batteryChannel: NTBatteryChannel?
    var signal: NTEegChannel?
    
    //MARK: views
    let connectBtn: UIButton = {
        let button = UIButton(type: .system)
        button.setTitle("Connect", for: .normal)
        button.setTitleColor(.green, for: .normal)
        button.titleLabel?.font = UIFont.systemFont(ofSize: 24)
        button.translatesAutoresizingMaskIntoConstraints = false
        button.titleLabel?.textAlignment = .center
        return button
    }()
    let signalBtn: UIButton = {
        let button = UIButton(type: .system)
        button.setTitle("Start signal", for: .normal)
        button.titleLabel?.font = UIFont.systemFont(ofSize: 24)
        button.translatesAutoresizingMaskIntoConstraints = false
        button.titleLabel?.textAlignment = .center
        button.isHidden = true
        return button
    }()
    let statusView:UIView = {
        let view = UIView ()
        view.translatesAutoresizingMaskIntoConstraints = false
        return view
    }()
    let zoomInButton: UIButton = {
        let button = UIButton(type: .roundedRect)
        button.setTitle("ZoomIn", for: .normal)
        button.titleLabel?.font = UIFont.systemFont(ofSize: 18)
        button.translatesAutoresizingMaskIntoConstraints = false
        return button
    }()
    let zoomOutButton: UIButton = {
        let button = UIButton(type: .roundedRect)
        button.setTitle("ZoomOut", for: .normal)
        button.titleLabel?.font = UIFont.systemFont(ofSize: 18)
        button.translatesAutoresizingMaskIntoConstraints = false
        return button
    }()
    let nameDeviceLabel: UILabel = {
        let view = UILabel()
        view.translatesAutoresizingMaskIntoConstraints = false
        view.font = UIFont.systemFont(ofSize: 24)
        view.textColor = .blue
        
        return view
    }()
    let batteryDeviceLabel: UILabel = {
        let view = UILabel()
        view.translatesAutoresizingMaskIntoConstraints = false
        view.font = UIFont.systemFont(ofSize: 24)
        view.textColor = .blue
        
        return view
    }()
    let plotView: UIView = {
        let view = UIView()
        view.isHidden = true
        return view
    }()
    //MARK: states properties
    var isSignal = false
    
    var vertexData: [Float] = []
    var scale = 1.0
    var startTimestamp = 0.0
    var founded = false {
        didSet {
            if(Thread.isMainThread) {
                connectBtn.isHidden = !founded
            } else {
                DispatchQueue.main.sync {
                    connectBtn.isHidden = !founded
                }
            }
        }
    }
    var connected = false {
        didSet {
            if( Thread.isMainThread) {
                if(connected) {
                    connectBtn.setTitle("Disconnect", for: .normal)
                    connectBtn.setTitleColor(.red, for: .normal)
                    
                    signalBtn.isHidden = false
                    signalBtn.setTitleColor(.green, for: .normal)
                    
                    statusView.isHidden = false
                    plotView.isHidden = false
                } else {
                    
                    connectBtn.setTitleColor(.green, for: .normal)
                    connectBtn.setTitle("Connect", for: .normal)
                    
                    signalBtn.isHidden = true
                    signalBtn.setTitleColor(.red, for: .normal)
                    statusView.isHidden = true
                    plotView.isHidden = true
                }
            }
            else {
                DispatchQueue.main.sync {
                    if(connected) {
                        connectBtn.setTitle("Disconnect", for: .normal)
                        connectBtn.setTitleColor(.red, for: .normal)
                        
                        signalBtn.isHidden = false
                        signalBtn.setTitleColor(.green, for: .normal)
                        
                        statusView.isHidden = false
                        plotView.isHidden = false
                    } else {
                        
                        connectBtn.setTitleColor(.green, for: .normal)
                        connectBtn.setTitle("Connect", for: .normal)
                        
                        signalBtn.isHidden = true
                        signalBtn.setTitleColor(.red, for: .normal)
                        statusView.isHidden = true
                        plotView.isHidden = true
                    }
                    
                }
            }
            
        }
    }
    
    
    //MARK: override
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view.
        // Configure the page view controller and add it as a child view controller.
        
        initUI()
        initMetal()
        initSystem()
        initCallback()
    }

    //MARK: init methods
    func initMetal() {
        
        self.metalDevice = MTLCreateSystemDefaultDevice()
        metalLayer = CAMetalLayer()             // 1
        metalLayer.device = metalDevice         // 2
        metalLayer.pixelFormat = .bgra8Unorm    // 3
        metalLayer.framebufferOnly = true       // 4
        metalLayer.frame = plotView.layer.frame // 5
        print (plotView.frame)
        plotView.layer.addSublayer(metalLayer)  // 6
        
        
        // 1
        let defaultLibrary = metalDevice.makeDefaultLibrary()!
        let fragmentProgram = defaultLibrary.makeFunction(name: "basic_fragment")
        let vertexProgram = defaultLibrary.makeFunction(name: "basic_vertex")
        
        // 2
        let pipelineStateDescriptor = MTLRenderPipelineDescriptor()
        pipelineStateDescriptor.vertexFunction = vertexProgram
        pipelineStateDescriptor.fragmentFunction = fragmentProgram
        pipelineStateDescriptor.colorAttachments[0].pixelFormat = .bgra8Unorm
        
        // 3
        pipelineState = try! metalDevice.makeRenderPipelineState(descriptor: pipelineStateDescriptor)
        commandQueue =  metalDevice.makeCommandQueue()
        timer = CADisplayLink(target: self, selector: #selector(loop_))
        timer.add(to: RunLoop.main, forMode: .common)
    }
    func initSystem() {
        scanner = NTDeviceScanner(.TypeCallibri)
        scanner!.subscribeFoundDevice { (findedDevice) in
            print("- founded device state -")
            self.device = findedDevice
            if let d = self.device {
                d.subscribeParameterChanged(subscriber: { (param) in
                    if( param == .State) {
                        let stateDevice = (self.device?.readParam(param: .State))! as NTState
                        self.connected = stateDevice == .Connected
                        DispatchQueue.main.sync {
                            self.connected = stateDevice == .Connected
                            if(self.connected) {
                                self.statusView.isHidden = false
                                self.nameDeviceLabel.text = (self.device?.readParam(param: .Name))! as String
                                var chnl: NTChannelInfo!
                                guard let lchannels = self.device!.channels() else {
                                    print("NTEegChannel: test (device == nil) ")
                                    return
                                }
                                
                                
                                for channel in lchannels {
                                    if( channel.type == .TypeSignal) {
                                        chnl = channel
                                        break
                                    }
                                    print("Device: ChannelInfo(", channel.index, channel.name, channel.type, ")\n")
                                }
                                
                                self.signal = NTEegChannel(self.device!, chnl)
                                
                                self.signal?.subscribeLengthChanged(subscribe: { (length) in
                                    let newdata = self.signal?.readData(offset: length-1, length: 1)?.first
                                    
                                    let duration = self.timer.targetTimestamp - self.timer.timestamp
                                    self.startTimestamp = self.startTimestamp + duration
                                    print(newdata, self.startTimestamp)
                                    self.vertexData.append(Float(self.startTimestamp))
                                    self.vertexData.append(Float(newdata!))
                                })
                                self.signalBtn.setTitle("Start Signal", for: .normal)
                                if let _ =  self.batteryChannel {
                                    print("NTBatteryChannel: - allraady init")
                                    
                                } else {
                                    self.batteryChannel = NTBatteryChannel(self.device)
                                    self.batteryChannel?.subscribeLengthChanged(subscribe: { (length) in
                                        let level = self.batteryChannel?.readData(offset: length-1, length: 1)?.first
                                        DispatchQueue.main.sync {
                                            self.batteryDeviceLabel.text = String(format: "Battery level: %d %", arguments: [level!])
                                        }
                                    })
                                }
                                
                            } else {
                                print("- disconnected - reconnect")
                            }
                        }
                    }
                })
            }
            self.founded = true
        }
        founded = false
    }
    func initCallback() {
        connectBtn.addTarget(self, action: #selector(connectPressed), for: .touchUpInside)
        signalBtn.addTarget(self, action: #selector(signalPressed), for: .touchUpInside)
    }
    func initUI() {
        view.backgroundColor = .lightGray
        view.addSubview(statusView)
        statusView.addSubview(nameDeviceLabel)
        statusView.addSubview(batteryDeviceLabel)
        
        view.addSubview(signalBtn)
        view.addSubview(connectBtn)
        
        view.addSubview(plotView)
        
        plotView.addSubview(zoomInButton);
        plotView.addSubview(zoomOutButton);
        
        zoomInButton.addTarget(self, action: #selector(zoopInPressed), for: .touchUpInside)
        zoomOutButton.addTarget(self, action: #selector(zoopOutPressed), for: .touchUpInside)
        NSLayoutConstraint.activate([
            zoomInButton.topAnchor.constraint(equalTo: plotView.topAnchor, constant: 40),
            zoomInButton.leftAnchor.constraint(equalTo: plotView.leftAnchor),
            zoomOutButton.topAnchor.constraint(equalTo: plotView.topAnchor, constant: 40),
            zoomOutButton.rightAnchor.constraint(equalTo: plotView.rightAnchor),
            ])
        
        let toppedding = 72  // cause 64 is height of statusView and 8 default padding
        let bottompedding = 180 + 32 // cause 180 is height of
        // (scannerBtn, connectBtn, signalBtn)
        plotView.frame = .init(x: 0, y: toppedding, width: Int(view.frame.width), height: Int(view.frame.height) - toppedding - bottompedding - 32)
        
        statusView.backgroundColor = .green
        statusView.isHidden = true
        NSLayoutConstraint.activate([
            statusView.topAnchor.constraint(equalTo: view.safeAreaLayoutGuide.topAnchor, constant: 8),
            statusView.heightAnchor.constraint(equalToConstant: 64),
            statusView.widthAnchor.constraint(equalTo: view.safeAreaLayoutGuide.widthAnchor),
            
            nameDeviceLabel.centerYAnchor.constraint(equalTo: statusView.centerYAnchor),
            nameDeviceLabel.leftAnchor.constraint(equalTo: statusView.leftAnchor, constant: 8),
            
            batteryDeviceLabel.centerYAnchor.constraint(equalTo: statusView.centerYAnchor),
            batteryDeviceLabel.rightAnchor.constraint(equalTo: statusView.rightAnchor, constant: -8),
            
            signalBtn.heightAnchor.constraint(equalToConstant: 60),
            signalBtn.centerXAnchor.constraint(equalTo: view.centerXAnchor),
            signalBtn.widthAnchor.constraint(equalTo: view.widthAnchor, multiplier: 0.65),
            signalBtn.bottomAnchor.constraint(equalTo: connectBtn.topAnchor, constant: 16),
            
            connectBtn.heightAnchor.constraint(equalToConstant: 60),
            connectBtn.centerXAnchor.constraint(equalTo: view.centerXAnchor),
            connectBtn.widthAnchor.constraint(equalTo: view.widthAnchor, multiplier: 0.65),
            connectBtn.bottomAnchor.constraint(equalTo: view.safeAreaLayoutGuide.bottomAnchor)
            
            ])
    }
    
    //MARK: main render
    func render() {
        // TODO
        if(vertexData.count == 0) {
            return
        }
        let dataSize = vertexData.count * MemoryLayout.size(ofValue: vertexData[0]) // 1
        vertexBuffer = metalDevice.makeBuffer(bytes: vertexData, length: dataSize, options: []) // 2
        
        guard let drawable = metalLayer?.nextDrawable() else { return }
        
        let renderPassDescriptor = MTLRenderPassDescriptor()
        renderPassDescriptor.colorAttachments[0].texture = drawable.texture
        renderPassDescriptor.colorAttachments[0].loadAction = .clear
        renderPassDescriptor.colorAttachments[0].clearColor = MTLClearColor(
            red:   0.0,
            green: 0.0,
            blue:  0.0,
            alpha: 1.0)
        
        let commandBuffer = commandQueue.makeCommandBuffer()!
        let renderEncoder = commandBuffer
            .makeRenderCommandEncoder(descriptor: renderPassDescriptor)!
        renderEncoder.setRenderPipelineState(pipelineState)
        renderEncoder.setVertexBuffer(vertexBuffer, offset: 0, index: 0)
    
        
        var curnenttime:[Float] = [Float(self.startTimestamp), Float(scale)]
        let curnenttimeBuffer = metalDevice.makeBuffer(bytes: curnenttime, length: 2*MemoryLayout.size(ofValue: curnenttime[0]), options: [])
        
        renderEncoder.setVertexBuffer(curnenttimeBuffer, offset: 0, index: 1)
        renderEncoder
            .drawPrimitives(type: .lineStrip, vertexStart: 0, vertexCount: vertexData.count / 3, instanceCount: 1)
        renderEncoder.endEncoding()
        
        commandBuffer.present(drawable)
        commandBuffer.commit()
    }
    
    //MARK: Buttons and Timer selectors
    @objc func loop_() {
        autoreleasepool {
            self.render()
        }
    }
    
    @objc func zoopInPressed() {
        scale  = scale * 10.0
    }
    @objc func zoopOutPressed() {
        scale  = scale / 10.0
    }
    
    @objc func connectPressed() {
        if let d = self.device {
            if( self.connected) {
                d.disconnect()
            } else {
                d.connect()
            }
        }
    }
    @objc func signalPressed() {
        isSignal = !isSignal
        if(isSignal) {
            signalSend()
        }
        else {
            stopsignalSend()
        }
        
    }

    
    //MARK: Signal Commands
    func signalSend() {
        print("- signal send -")
        self.device?.execute(command: .StartSignal)
        startTimestamp = 0
        vertexData = []
        signalBtn.setTitle("Stop Signal", for: .normal)
        if( Thread.isMainThread) {
            signalBtn.setTitle("Stop Signal", for: .normal)
        }
        else {
            DispatchQueue.main.sync {
                signalBtn.setTitle("Stop Signal", for: .normal)
                
            }
        }
    }
    func stopsignalSend() {
        print("- signal stop send -")
        self.device?.execute(command: .StopSignal)
        vertexData = []
        startTimestamp = 0.0
        if( Thread.isMainThread) {
            signalBtn.setTitle("Start Signal", for: .normal)
        }
        else {
            DispatchQueue.main.sync {
                signalBtn.setTitle("Start Signal", for: .normal)
                
            }
        }
        
    }
    
}

