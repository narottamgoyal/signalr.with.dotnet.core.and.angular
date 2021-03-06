import { Component, OnInit } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  private hubConnection: HubConnection;

  public ngOnInit() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:50930/pushNotification').build();

    this.hubConnection.start().then(() => {
      console.log('connection started');
    }).catch(err => console.log(err));

    this.hubConnection.onclose(() => {
      debugger;
      setTimeout(() => {
        console.log('try to re start connection');
        debugger;
        this.hubConnection.start().then(() => {
          debugger;
          console.log('connection re started');
        }).catch(err => console.log(err));
      }, 5000);
    });

    this.hubConnection.on('privateMessageMethodName', (data) => {
      debugger;
      console.log('private Message:' + data);
    });

    this.hubConnection.on('publicMessageMethodName', (data) => {
      debugger;
      console.log('public Message:' + data);
    });

    this.hubConnection.on('clientMethodName', (data) => {
      debugger;
      console.log('server message:' + data);
    });

    this.hubConnection.on('WelcomeMethodName', (data) => {
      debugger;
      console.log('client Id:' + data);
      this.hubConnection.invoke('GetDataFromClient', 'abc@abc.com', data).catch(err => console.log(err));
    });
  }

  public stopConnection() {
    this.hubConnection.stop().then(() => {
      console.log('stopped');
    }).catch(err => console.log(err));
  }
}
