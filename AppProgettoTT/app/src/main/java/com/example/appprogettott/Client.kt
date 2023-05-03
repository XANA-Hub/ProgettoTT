package com.example.appprogettott

import android.os.AsyncTask
import android.util.Log
import java.io.BufferedReader
import java.io.InputStreamReader
import java.net.DatagramPacket
import java.net.DatagramSocket
import java.net.Socket

class Client : AsyncTask<String?, String?, String?>() {

    override fun doInBackground(vararg params: String?): String? {
        var socketTcp: Socket? = null
        var socketUdp: DatagramSocket? = null

        try {
            // crea la socket TCP e connettiti al server
            socketTcp = Socket("192.168.178.149", 25565)

            // crea lo stream di input e leggi la risposta del server dalla socket TCP
            val inStream = BufferedReader(InputStreamReader(socketTcp.getInputStream()))
            val tcpResponse = inStream.readLine()
            Log.d("SUPER_PALLE_TCP", "TCP response: $tcpResponse")


            // crea la socket UDP
            socketUdp = DatagramSocket(25564)

            // leggi la risposta dal server sulla socket UDP
            val receiveBuffer = ByteArray(1024)
            val receivePacket = DatagramPacket(receiveBuffer, receiveBuffer.size)
            socketUdp.receive(receivePacket)
            val udpResponse = String(receivePacket.data, 0, receivePacket.length)
            Log.d("SUPER_PALLE_UDP", "UDP response: $udpResponse")


        } catch (e: Exception) {
            e.printStackTrace()
        } finally {
            // chiudi le socket
            socketTcp?.close()
            socketUdp?.close()
        }
        return null
    }
}
