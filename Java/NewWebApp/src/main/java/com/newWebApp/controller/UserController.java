package com.newWebApp.controller;

import javax.servlet.http.HttpServletRequest;

import com.newWebApp.pojo.User;
import com.newWebApp.service.IUserService;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.ResponseBody;


import java.util.HashMap;
import java.util.Map;

@Controller
@RequestMapping("/user")
public class UserController {

    @Autowired
    private IUserService userService;
    @RequestMapping("/showUser")
    @ResponseBody
    public Map<Object , Object> getAnaloghistory(){
        Map<Object , Object> result = new HashMap<Object , Object>();
        Integer userId = 1;
        User user = this.userService.selectUser(userId);
        result.put("test",user.getName());
        return result;
    }

}