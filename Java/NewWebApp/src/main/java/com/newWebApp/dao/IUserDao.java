package com.newWebApp.dao;


import org.apache.ibatis.annotations.Param;
import org.springframework.stereotype.Repository;
import com.newWebApp.pojo.User;


@Repository
public interface IUserDao {

    User selectByPrimaryKey(Integer id);
}