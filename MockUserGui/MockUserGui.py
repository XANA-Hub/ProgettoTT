import socket
from time import sleep
import tkinter as tk

HOST = "localhost"        # The server's hostname or IP address
PORT = 25565              # The port used by the server

#command
forwardStartCommand = b"ID:000;TYPE:Movement;BODY:Forward Start"
forwardStopCommand = b"ID:000;TYPE:Movement;BODY:Forward Stop"
backwardStartCommand = b"ID:000;TYPE:Movement;BODY:Backward Start"
backwardStopCommand = b"ID:000;TYPE:Movement;BODY:Backward Stop"
rotateLeftStartCommand = b"ID:000;TYPE:Movement;BODY:Rotate_Left Start"
rotateLeftStopCommand = b"ID:000;TYPE:Movement;BODY:Rotate_Left Stop"
rotateRightStartCommand = b"ID:000;TYPE:Movement;BODY:Rotate_Right Start"
rotateRightStopCommand = b"ID:000;TYPE:Movement;BODY:Rotate_Right Stop"
armDownCommand = b"ID:000;TYPE:Robot_Arm;BODY:Down"
armUpCommand = b"ID:000;TYPE:Robot_Arm;BODY:Up"
grabCommand = b"ID:000;TYPE:Robot_Arm;BODY:Grab"
releaseCommand = b"ID:000;TYPE:Robot_Arm;BODY:Release"
DisconnectCommand = b"ID:000;TYPE:Disconnect;BODY:Disconnect"
ConnectedCommand = b"ID:000;TYPE:Start;BODY:192.168.1.238:25565"    #cambia a seconda di chi lo lancia

#socket connection --------------------------------------------------------------------------------
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect((HOST, PORT))
print("connected with socket server")

print("in attesa di risposta dal robot")
s.recv(1024)
print("robot pronto inizio invio dei comandi")
s.sendall(ConnectedCommand)


#tkinter event handler -----------------------------------------------------------------------
def forward_pressed(event):
    s.sendall(forwardStartCommand)
def forward_release(event):
    s.sendall(forwardStopCommand)

def backward_pressed(event):
    s.sendall(backwardStartCommand)
def backward_release(event):
    s.sendall(backwardStopCommand)

def turnleft_pressed(event):
    s.sendall(rotateLeftStartCommand)
def turnleft_release(event):
    s.sendall(rotateLeftStopCommand)

def turnright_pressed(event):
    s.sendall(rotateRightStartCommand)
def turnright_release(event):
    s.sendall(rotateRightStopCommand)

def armup_pressed(event):
    s.sendall(armUpCommand)
def armdown_pressed(event):
    s.sendall(armDownCommand)
def grab_pressed(event):
    s.sendall(grabCommand)
def release_pressed(event):
    s.sendall(releaseCommand)

def disconnect_pressed(event):
    s.sendall(DisconnectCommand)
    window.quit()

#tkinter setup ------------------------------------------------------------------------------------
window = tk.Tk()

#add a text
greeting = tk.Label(text="TankToccia ready for work",
    height=2,
    fg="white",
    bg="black"
)  
greeting.pack(fill=tk.X)     #add the widget to the window


#movement control frame
movementFrameLeft = tk.Frame(master=window, width=75, height=10, bg="gray")
movementFrameLeft.pack(fill=tk.Y, side=tk.LEFT)

movementFrameCenter = tk.Frame(master=window, width=75, height=10, bg="gray")
movementFrameCenter.pack(fill=tk.Y, side=tk.LEFT)

movementFrameRight = tk.Frame(master=window, width=75, height=10, bg="gray")
movementFrameRight.pack(fill=tk.Y, side=tk.LEFT)

#movement crane frame
craneFrameLeft = tk.Frame(master=window, width=75, height=10, bg="gray")
craneFrameLeft.pack(fill=tk.Y, side=tk.RIGHT)

craneFrameCenter = tk.Frame(master=window, width=75, height=10, bg="gray")
craneFrameCenter.pack(fill=tk.Y, side=tk.RIGHT)

craneFrameRight = tk.Frame(master=window, width=75, height=10, bg="gray")
craneFrameRight.pack(fill=tk.Y, side=tk.RIGHT)


#definizione bottoni movement
buttonForward = tk.Button(
    master=movementFrameCenter,
    text="Forward",
    width=10,
    height=5,
    bg="white",
    fg="black",
)
buttonForward.pack()
buttonForward.bind("<ButtonPress-1>", forward_pressed)
buttonForward.bind("<ButtonRelease-1>", forward_release)

buttonBackward = tk.Button(
    master=movementFrameCenter,
    text="Backward",
    width=10,
    height=5,
    bg="white",
    fg="black",
)
buttonBackward.pack()
buttonBackward.bind("<ButtonPress-1>", backward_pressed)
buttonBackward.bind("<ButtonRelease-1>", backward_release)

buttonLeft = tk.Button(
    master=movementFrameLeft,
    text="Left",
    width=10,
    height=5,
    bg="white",
    fg="black",
)
buttonLeft.pack(side=tk.BOTTOM)
buttonLeft.bind("<ButtonPress-1>", turnleft_pressed)
buttonLeft.bind("<ButtonRelease-1>", turnleft_release)

buttonRight = tk.Button(
    master=movementFrameRight,
    text="Right",
    width=10,
    height=5,
    bg="white",
    fg="black",
)
buttonRight.pack(side=tk.BOTTOM)
buttonRight.bind("<ButtonPress-1>", turnright_pressed)
buttonRight.bind("<ButtonRelease-1>", turnright_release)


#definizione bottoni crane
buttonUp = tk.Button(
    master=craneFrameCenter,
    text="Up",
    width=10,
    height=5,
    bg="white",
    fg="black",
)
buttonUp.pack()
buttonUp.bind("<ButtonPress-1>", armup_pressed)

buttonDown = tk.Button(
    master=craneFrameCenter,
    text="Down",
    width=10,
    height=5,
    bg="white",
    fg="black",
)
buttonDown.pack()
buttonDown.bind("<ButtonPress-1>", armdown_pressed)

buttonGrab = tk.Button(
    master=craneFrameRight,
    text="Grab",
    width=10,
    height=5,
    bg="white",
    fg="black",
)
buttonGrab.pack(side=tk.BOTTOM)
buttonGrab.bind("<ButtonPress-1>", grab_pressed)

buttonRelease = tk.Button(
    master=craneFrameLeft,
    text="Release",
    width=10,
    height=5,
    bg="white",
    fg="black",
)
buttonRelease.pack(side=tk.BOTTOM)
buttonRelease.bind("<ButtonPress-1>", release_pressed)

buttonRelease = tk.Button(
    master=window,
    text="Disconnect",
    width=10,
    height=5,
    bg="white",
    fg="black",
)
buttonRelease.pack(side=tk.BOTTOM)
buttonRelease.bind("<ButtonPress-1>", disconnect_pressed)


window.mainloop() #to keep the window open
#This method listens for events, such as button clicks or keypresses
#and blocks any code that comes after it from running until you close the window where you called the method




