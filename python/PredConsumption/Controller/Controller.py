from flask import Flask
from flask import Flask,render_template,request,redirect,session
import json

from PredConsumption.PredictionService import DataProcessService
from PredConsumption.PredictionService import RegressionService
app = Flask(__name__)

@app.route('/')
def hello_world():
    return 'Hello World!'

@app.route("/training",methods=["GET","POST"])  # 指定该路由可接收的请求方式，默认为GET
def training():
    if request.method == 'POST':
        data=request.data
        inp_dict = json.loads(data)
        value, feature=DataProcessService.getDataFromDict(inp_dict)
        #print(inp_dict)
       # result = RegressionService.modelsSelect(value, feature)
        mse, r2=RegressionService.RegressionAdapt(feature,value, 'GradientBoost', plotSwitch=False)
        return str(r2)
    else:
        method = request.args.get('method')
        return method
if __name__ == '__main__':
    app.run(debug=True)