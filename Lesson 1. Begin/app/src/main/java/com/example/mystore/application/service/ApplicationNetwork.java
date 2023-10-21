package com.example.mystore.application.service;

import com.example.mystore.application.network.CategoriesApi;

import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class ApplicationNetwork {
    private static ApplicationNetwork instance;
    private Retrofit retrofit;

    public ApplicationNetwork() {
        retrofit = new Retrofit
                .Builder()
                .baseUrl("https://spu123.itstep.click")
                .addConverterFactory(GsonConverterFactory.create())
                .build();
    }

    public static ApplicationNetwork getInstance() {
        if(instance==null)
            instance=new ApplicationNetwork();
        return instance;
    }
    public CategoriesApi getCategoriesApi() {
        return retrofit.create(CategoriesApi.class);
    }
}
