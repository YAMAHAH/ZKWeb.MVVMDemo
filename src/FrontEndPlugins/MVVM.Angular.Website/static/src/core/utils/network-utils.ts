// export class NetWorkUtils {

//     /**
//      * 查找本地IP地址
//      * 用法: findIP.then(ip => console.debug('your ip: ', ip))
//      *        .catch(e => console.error(e))
//      */
//     static findIP() {
//         return new Promise(r => {
//             let w = window as any,
//                 a = new (w.RTCPeerConnection || w.mozRTCPeerConnection || w.webkitRTCPeerConnection)({ iceServers: [] }),
//                 b = () => { };
//             a.createDataChannel("");
//             a.createOffer(c => a.setLocalDescription(c, b, b), b);
//             a.onicecandidate = c => {
//                 try {
//                     c.candidate.candidate.match(/([0-9]{1,3}(\.[0-9]{1,3}){3}|[a-f0-9]{1,4}(:[a-f0-9]{1,4}){7})/g)
//                         .forEach(r)
//                 }
//                 catch (e) { }
//             }
//         })
//     }
// }

// function findIP(onNewIP) { //  onNewIp - your listener function for new IPs
//     var promise = new Promise(function (resolve, reject) {
//         try {
//             var myPeerConnection = RTCPeerConnection || (Window as any).mozRTCPeerConnection || webkitRTCPeerConnection; //compatibility for firefox and chrome
//             var pc = new myPeerConnection({ iceServers: [] }),
//                 noop = function () { },
//                 localIPs = {},
//                 ipRegex = /([0-9]{1,3}(\.[0-9]{1,3}){3}|[a-f0-9]{1,4}(:[a-f0-9]{1,4}){7})/g,
//                 key;
//             function ipIterate(ip) {
//                 if (!localIPs[ip]) onNewIP(ip);
//                 localIPs[ip] = true;
//             }
//             pc.createDataChannel(""); //create a bogus data channel
//             pc.createOffer(function (sdp) {
//                 sdp.sdp.split('\n').forEach(function (line) {
//                     if (line.indexOf('candidate') < 0) return;
//                     line.match(ipRegex).forEach(ipIterate);
//                 });
//                 pc.setLocalDescription(sdp, noop, noop);
//             }, noop); // create offer and set local description

//             pc.onicecandidate = function (ice) { //listen for candidate events
//                 if (ice && ice.candidate && ice.candidate.candidate && ice.candidate.candidate.match(ipRegex)) {
//                     ice.candidate.candidate.match(ipRegex).forEach(ipIterate);
//                 }
//                 resolve("FindIPsDone");
//                 return;
//             };
//         }
//         catch (ex) {
//             reject(Error(ex));
//         }
//     });// New Promise(...{ ... });
//     return promise;
// };
// //This is the callback that gets run for each IP address found
// function foundNewIP(ip) {
//     if (typeof window['ipAddress'] === 'undefined') {
//         window['ipAddress'] = ip;
//     }
//     else {
//         window['ipAddress'] += " - " + ip;
//     }
// }

// //This is How to use the Waitable findIP function, and react to the
// //results arriving
// var ipWaitObject = findIP(foundNewIP);        // Puts found IP(s) in window.ipAddress
// ipWaitObject.then(
//     function (result) {
//         alert("IP(s) Found.  Result: '" + result + "'. You can use them now: " + window['ipAddress'])
//     },
//     function (err) {
//         alert("IP(s) NOT Found.  FAILED!  " + err)
//     }
// );

