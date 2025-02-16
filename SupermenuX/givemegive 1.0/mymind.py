import socket
import sys
import pyautogui
import winsound
from pynput import keyboard
import threading
import time

def on_press(key):
    try:
        if key == keyboard.Key.esc:
            return False
    except AttributeError:
        pass

def udp_client(ip_address):
    try:
        server_ip = "192.168.0.122"
        port = 1337  # Choose an appropriate port

        client_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        client_socket.bind((server_ip, port))
                
        # Set a timeout for the socket
        client_socket.settimeout(0.1)
        
        # Set up the keyboard listener in a separate thread
        listener = keyboard.Listener(on_press=on_press)
        listener.start()
        
        # Send a test message to the server
        message = "Hello, Server!".encode('utf-8')
        client_socket.sendto(message, (ip_address, port))
        print(f"Connected to server at {ip_address}:{port}")
        print("Waiting for responses. Press Esc to stop.")
        
        while listener.is_alive():
            try:
                response, server_address = client_socket.recvfrom(128)
                decoded_response = response.decode('utf-8').strip()
                print(f"Received: {decoded_response}")
                
                # Compare the decoded response and perform actions
                if decoded_response == "l":
                    pyautogui.press('left')
                    print("Pressed left")
                elif decoded_response == "r":
                    pyautogui.press('right')
                    print("Pressed right")
                    
            except socket.timeout:
                # No data received, continue listening
                continue
            except Exception as e:
                print(f"Error receiving data: {e}")
                continue
        
        # If we exit the loop, play sound and clean up
        frequency = 400
        duration = 10
        winsound.Beep(frequency, duration)
        
    except Exception as e:
        print(f"Error: {e}")
    finally:
        print("Closing connection...")
        client_socket.close()
        sys.exit(0)

if __name__ == "__main__":
    # Default IP if none provided
    default_ip = ""
    
    if len(sys.argv) != 2:
        print(f"No IP address provided. Using default: {default_ip}")
        server_ip = default_ip
    else:
        server_ip = sys.argv[1]
        
    try:
        udp_client(server_ip)
    except KeyboardInterrupt:
        print("\nProgram terminated by user")
        sys.exit(0)