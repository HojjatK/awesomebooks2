import { Injectable } from '@angular/core';

export enum LogLevel {
    OFF = 0,
    ERROR = 1,
    WARN = 2,
    INFO = 3,
    DEBUG = 4
}

type ILoggable = (any)[];

@Injectable()
export class LoggerService {
    loggerMsgLabel = "AwesomeBooks";//environment.appName + ':' + environment.appVersion + ' - ';
    displaylogLevel: LogLevel = LogLevel.DEBUG; // come from environment

    isErrorEnabled = () => this.displaylogLevel >= LogLevel.ERROR;
    isWarnEnabled = () => this.displaylogLevel >= LogLevel.WARN;
    isInfoEnabled = () => this.displaylogLevel >= LogLevel.INFO;
    isDebugEnabled = () => this.displaylogLevel >= LogLevel.DEBUG;

    setLogLevel(log: LogLevel) {
      this.displaylogLevel = log;
    }

    info(...msg: ILoggable) {
        const output = [this.currentDate(), this.loggerMsgLabel, ...msg].join(' ');
        if (this.isInfoEnabled()) {
          console.info(output);
        }
      }
    
      debug(...msg: ILoggable) {
        const output = [this.currentDate(), this.loggerMsgLabel].join(' ');
        if (this.isDebugEnabled()) {
          console.debug(output, ...msg);
        }
      }
    
      warn(...msg: ILoggable) {
        const output = [this.currentDate(), this.loggerMsgLabel, ...msg].join(' ');
        if (this.isWarnEnabled()) {
          console.warn(output);
        }
      }
    
      error(...msg: ILoggable) {
        const output = [this.currentDate(), this.loggerMsgLabel, ...msg].join(' ');
        if (this.isErrorEnabled()) {
          console.error(output);
        }
      }

      private currentDate() {
        return Math.floor(Date.now());
      }
}