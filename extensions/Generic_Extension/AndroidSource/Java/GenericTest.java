package ${YYAndroidPackageName};

import android.util.Log;
import java.io.File;
import java.io.FileReader;
import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.lang.String;

import ${YYAndroidPackageName}.R;
import com.yoyogames.runner.RunnerJNILib;


public class GenericTest
{
	private static final int EVENT_OTHER_SOCIAL = 70;
        public void RokuScan() throws Exception {
            /* create byte arrays to hold our send and response data */
            byte[] sendData = new byte[1024];
            byte[] receiveData = new byte[1024];

            /* our M-SEARCH data as a byte array */
            String MSEARCH = "M-SEARCH * HTTP/1.1\nHost: 239.255.255.250:1900\nMan: \"ssdp:discover\"\nST: roku:ecp\n"; 
            sendData = MSEARCH.getBytes();

            /* create a packet from our data destined for 239.255.255.250:1900 */
            DatagramPacket sendPacket = new DatagramPacket(sendData, sendData.length, InetAddress.getByName("239.255.255.250"), 1900);

            /* send packet to the socket we're creating */
            DatagramSocket clientSocket = new DatagramSocket();
            clientSocket.send(sendPacket);

            /* recieve response and store in our receivePacket */
            DatagramPacket receivePacket = new DatagramPacket(receiveData, receiveData.length);
            clientSocket.receive(receivePacket);

            /* get the response as a string */
            String response = new String(receivePacket.getData());
			response = response.split("http://")[1];
			response = response.split(":")[0];

            /* print the response */
			int dsMapIndex = RunnerJNILib.jCreateDsMap(null, null, null);
			RunnerJNILib.DsMapAddString( dsMapIndex, "ip", response );
			RunnerJNILib.CreateAsynEventWithDSMap(dsMapIndex, EVENT_OTHER_SOCIAL);
            /*return response;*/
        }
} // End of class

